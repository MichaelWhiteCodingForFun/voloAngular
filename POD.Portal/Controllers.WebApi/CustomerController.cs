using Ninject;
using POD.Entities;
using POD.Interfaces;
using POD.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using POD.Data;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using POD.Portal.Helpers;
using Microsoft.AspNet.Identity;
using POD.Portal.Models.Auth;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Web.Http.Results;
using POD.Entities.Enums;

namespace POD.Portal.Controllers.WebApi
{
    [RoutePrefix("api/customers")]
    public class CustomerServiceController : CommonController
    {
        #region Inject & ctor
        [Inject]
        public ICustomerDataService _customerDataService { get; set; }
        [Inject]
        public IUserDataService _userDataService { get; set; }
        public CustomerServiceController(ICustomerDataService customerDataService, IUserDataService userDataService)
        {
            _customerDataService = customerDataService;
            _userDataService = userDataService;
        }
        #endregion



        #region WebApi
        /// <summary>
        /// Get Customers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="customerViewModel"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "System Admin, Company Admin")]
        [Route("GetCustomers")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCustomers(HttpRequestMessage request, [FromBody] CustomersViewModel customersViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                CustomersViewModel result = new CustomersViewModel();
                var currentLoggedInUser = UserHelper.GetUserInfo(this);
              
                int currentPageNumber = customersViewModel.CurrentPageNumber;
                int pageSize = customersViewModel.PageSize;
                string sortExpression = customersViewModel.SortExpression;
                string sortDirection = customersViewModel.SortDirection;

                int totalRows;
                List<Organization> customers = _customerDataService.GetCustomers(currentLoggedInUser.OrganizationID, customersViewModel.AccountNumber, customersViewModel.AccountName, customersViewModel.AdminUser, customersViewModel.UserName, customersViewModel.StatusName, customersViewModel.LastLoginDate,
                    currentPageNumber, pageSize, sortExpression, sortDirection, out totalRows);
                // customerViewModelList = new CustomerViewModelList();

                //foreach (Customer customer in customers)
                //{
                //    CustomerViewModel customerViewModel = customer;
                //    customerViewModelList.Add(customerViewModel);
                //}


                var customerViewModelList = customers.Select(user => (CustomerViewModel)user);
                result.Customers = customerViewModelList.ToList();
                result.TotalRows = totalRows;

                response = ResponseMessage(Request.CreateResponse<CustomersViewModel>(HttpStatusCode.OK, result));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;

        }
        public static int CalculateTotalPages(long numberOfRecords, Int32 pageSize)
        {
            long result;
            int totalPages;
            Math.DivRem(numberOfRecords, pageSize, out result);
            if (result > 0)
                totalPages = (int)((numberOfRecords / pageSize)) + 1;
            else
                totalPages = (int)(numberOfRecords / pageSize);
            return totalPages;

        }

        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("Add")]
        [HttpPost]
        public async Task<IHttpActionResult> Create(HttpRequestMessage request, [FromBody] CustomerViewModel customerViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                var currentLoggedInUser = UserHelper.GetUserInfo(this);

                Organization newOrganization = (CustomerViewModel)customerViewModel;
                newOrganization.CreatedDateUtc = DateTime.Now;
                newOrganization.CreatedByID = currentLoggedInUser.ID;
                newOrganization.ParentOrganizationID = currentLoggedInUser.OrganizationID;
                newOrganization.IsActive = true;

                switch ((OrganizationType)currentLoggedInUser.OrganizationTypeID)
                {
                    case OrganizationType.System:
                        newOrganization.OrganizationTypeID = (int)OrganizationType.Company;
                        break;
                    case OrganizationType.Company:
                        newOrganization.OrganizationTypeID = (int)OrganizationType.Customer;
                        break;
                }               

                bool success = _customerDataService.Create(newOrganization);

                Guid userID = new Guid();
                if (success)
                {
                    //get admin role ID
                    
                    var roles = _userDataService.Roles();
                    var adminRole = roles.FirstOrDefault(r => r.IsAdmin);
                    //set user's role and organization datas
                    customerViewModel.AdminUser.Role = new UserRole() { ID = adminRole.ID, IsPrimary = true };
                    customerViewModel.AdminUser.Organization = new UserOrganization() { ID = adminRole.ID, OrganizationID = newOrganization.ID };

                    UserViewModel model = customerViewModel.AdminUser;
                    ////registerUser
                    //var user = UserHelper.CreateCustomIdentityUser(this, model);
                    //var result = await UserManager.CreateAsync(user);
                    //if (result.Succeeded)
                    //{
                    //    await SendPasswordResetEmail(user, true);
                    //    //await SendConfirmationEmail(user);
                    //    response = Ok();
                    //}
                    //else
                    //{
                    //    IHttpActionResult errorResult = GetErrorResult(result);

                    //    if (errorResult != null)
                    //    {
                    //        response = errorResult;
                    //    }
                    //}

                    response = await base.RegisterUser(model);

                    if (response is OkResult || response is OkNegotiatedContentResult<string>)
                        response = Ok(Resource.CustomerAddedSuccessfully);

                    //HttpClientHandler handler = new HttpClientHandler
                    //{
                    //    UseCookies = true,
                    //    UseDefaultCredentials = true
                    //};

                    //using (var client = new HttpClient(handler))
                    //{
                    //    HttpResponseMessage responseO;

                    //    client.BaseAddress = new Uri("http://local.pod.com/");



                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //    responseO = await client.PostAsJsonAsync("api/accounts/Register", customerViewModel.AdminUser);
                    //    if (responseO.IsSuccessStatusCode)
                    //    {
                    //        User user = _userDataService.GetUserByUsername(customerViewModel.AdminUser.UserName);
                    //        userID = user.ID;
                    //    }
                    //}
                    //response = Ok();
                }
                else
                {
                    response = BadRequest();
                }
            }
            catch (Exception e)
            {
                string message = UserHelper.ErrorHandler(e);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, message));
            }
            return response;
        }

        [Authorize(Roles = "System Admin, Company Admin")]
        [Route("Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> Edit([FromBody] CustomerViewModel customerViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                Organization customer = (CustomerViewModel)customerViewModel;
                _customerDataService.Edit(customer);
                response = ResponseMessage(Request.CreateResponse<Organization>(HttpStatusCode.OK, customer));
            }
            catch (Exception e)
            {
                string message = UserHelper.ErrorHandler(e);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, message));
            }
            return response;
        }

        //[Authorize]
        //[Route("Get")]
        //[HttpPost]
        //public async Task<IHttpActionResult> GetCustomerByID([FromBody]CustomerViewModel customerViewModel)
        //{
        //    IHttpActionResult response = null;
        //    try
        //    {
        //        Organization customer = _customerDataService.GetCustomerByID(customerViewModel.ID);
        //        response = ResponseMessage(Request.CreateResponse<Organization>(HttpStatusCode.OK, customer));
        //    }
        //    catch (Exception e)
        //    {
        //        string message = UserHelper.ErrorHandler(e);
        //        response = ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, message));
        //    }
        //    return response;
        //}

        [Authorize]
        [Route("GetByName")]
        [HttpPost]
        public async Task<IHttpActionResult> GetByName([FromBody]CustomerViewModel customerViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                Organization customer = _customerDataService.GetCustomerByName(customerViewModel.OrganizationName);
                response = ResponseMessage(Request.CreateResponse<CustomerViewModel>(HttpStatusCode.OK, customerViewModel));
            }
            catch (Exception e)
            {
                string message = UserHelper.ErrorHandler(e);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, message));
            }
            return response;
        }

        [Authorize(Roles = "System Admin, Company Admin")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IHttpActionResult> Delete([FromBody]CustomerViewModel customerViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                _customerDataService.Delete(customerViewModel.ID);
                response = ResponseMessage(Request.CreateResponse<CustomerViewModel>(HttpStatusCode.OK, customerViewModel));
            }
            catch (Exception e)
            {
                string message = UserHelper.ErrorHandler(e);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, message));
            }
            return response;
        }

        [Authorize(Roles = "System Admin, Company Admin")]
        [Route("GetOrganizationCustomers")]
        [HttpPost]
        public async Task<IHttpActionResult> GetOrganizationCustomers([FromBody]CustomerViewModel customerViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                UserIdentityInfo userinfo = UserHelper.GetUserInfo(this);
                Guid currentOrganizationID = userinfo.OrganizationID;
                List<Organization> Organizations =  _customerDataService.GetOrganizationsByParentID(currentOrganizationID);
                response = ResponseMessage(Request.CreateResponse<List<Organization>>(HttpStatusCode.OK, Organizations));
            }
            catch (Exception e)
            {
                string message = UserHelper.ErrorHandler(e);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, message));
            }
            return response;
        }
        #endregion
    }
}
