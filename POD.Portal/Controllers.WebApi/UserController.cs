using Ninject;
using POD.Entities;
using POD.Entities.Enums;
using POD.Interfaces;
using POD.Portal.Helpers;
using POD.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace POD.Portal.Controllers.WebApi
{
    [RoutePrefix("api/users")]
    public class UserController : CommonController
    {
        [Inject]
        public IUserDataService _userDataService { get; set; }
        public UserController(IUserDataService userDataService)
        {
            _userDataService = userDataService;

            //int a;
            //_userDataService.GetUsers(1, 100, "", "",out a);
        }

        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("GetUsers")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUsers(HttpRequestMessage request, [FromBody] UsersViewModel usersViewModel)
        {
            //throw new Exception("qas");
            IHttpActionResult response = null;
            try
            {
                UsersViewModel result = new UsersViewModel();
                UserIdentityInfo userinfo = UserHelper.GetUserInfo(this);
                if (userinfo != null)
                {
                    //if the OrganizationID comes with the request, than it is a Company admin in Customer view, else search with the current logged in OrganizationID
                    Guid organizationID = (usersViewModel.OrganizationID == null || usersViewModel.OrganizationID == default(Guid)) ? userinfo.OrganizationID : usersViewModel.OrganizationID.Value;         

                    int currentPageNumber = usersViewModel.CurrentPageNumber;
                    int pageSize = usersViewModel.PageSize;
                    string sortExpression = usersViewModel.SortExpression;
                    string sortDirection = usersViewModel.SortDirection;
                    int totalRows;
                    var userlist = _userDataService.GetUsers(organizationID, usersViewModel.OrganizationName, usersViewModel.FullName, usersViewModel.UserName, usersViewModel.RoleName, usersViewModel.StatusName, usersViewModel.LastLoginDate,
                                                            currentPageNumber, pageSize, sortExpression, sortDirection, out totalRows);

                    var userViewModelList = userlist.Select(user =>
                    {
                        var userVM = (UserViewModel)user;
                        /*if the user is current user or disabled, it is not editable */
                        userVM.IsEditable = (userVM.ID == userinfo.ID) ? false : true;
                        return userVM;
                    });           

                    result.Users = userViewModelList.ToList();                  
                    result.TotalRows = totalRows;
                }
                response = ResponseMessage(Request.CreateResponse<UsersViewModel>(HttpStatusCode.OK, result));

            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("Roles")]
        [HttpPost]
        public async Task<IHttpActionResult> Roles()
        {
            IHttpActionResult response = null;
            try
            {
                var currentLoggedInUser = UserHelper.GetUserInfo(this);
                var roles = _userDataService.Roles();
                response = ResponseMessage(Request.CreateResponse<IEnumerable<Role>>(HttpStatusCode.OK, roles));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("AccountStatuses")]
        [HttpPost]
        public async Task<IHttpActionResult> AccountStatuses()
        {
            IHttpActionResult response = null;
            try
            {
                Dictionary<int, string> accountStatuses = new Dictionary<int, string>();
                foreach (AccountStatus accountStatus in Enum.GetValues(typeof(AccountStatus)))
                {
                    if (accountStatus == AccountStatus.Inactive || accountStatus == AccountStatus.Locked || accountStatus == AccountStatus.None)
                        continue;
                    accountStatuses.Add((int)accountStatus, accountStatus.ToString());
                }
                response = ResponseMessage(Request.CreateResponse<Dictionary<int, string>>(HttpStatusCode.OK, accountStatuses));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [AllowAnonymous]
        [Route("GetUserNameById/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserNameById(Guid id)
        {
            //string userName = userViewModel.UserID != null ? _userDataService.GetUserByID(userViewModel.UserID).UserName : null;
            IHttpActionResult response = null;
            try
            {
                User user = _userDataService.GetUserByID(id);
                UserViewModel resultUser = new UserViewModel() { UserName = user.UserName, StatusID = user.StatusID };
                if (user != null)
                {
                    string userName = user.UserName;
                    response = Ok<UserViewModel>(resultUser);
                }
                else
                {
                    response = BadRequest();
                }
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Authorize]
        [Route("GetUser")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUser(HttpRequestMessage request, [FromBody] UserViewModel userViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                Guid Id = userViewModel.ID;
                var user = _userDataService.GetUserByID(Id);
                response = ResponseMessage(Request.CreateResponse<User>(HttpStatusCode.OK, user));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }
        
        [Route("GetUserIdentity")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserIdentity()
        {
            IHttpActionResult response = null;
            try
            {
                UserIdentityInfo userInfo = UserHelper.GetUserInfo(this);
                response = ResponseMessage(Request.CreateResponse<UserIdentityInfo>(HttpStatusCode.OK, userInfo));
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("UpdateUser")]
        [HttpPost]
        public async Task<IHttpActionResult> Update([FromBody] UserViewModel userViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                // return null;
                //for dapper
                //userViewModel.Role.RoleID = userViewModel.Role.ID;
                //test
              //  userViewModel.StatusID = 2;
                //role has been changed
                if (userViewModel.Role.ID != default(Guid))
                    userViewModel.Role.RoleID = userViewModel.Role.ID;

                User existingUser = _userDataService.GetUserByID(userViewModel.ID);

                if (existingUser != null)
                {
                    if (String.Compare(userViewModel.UserName, existingUser.UserName) == 0)
                    //username has not been changed
                    {                        
                        _userDataService.Update((User)userViewModel);
                    }
                    else
                    //user name has been change, old user should be deleted, new one created
                    {
                        //make old user not primary
                        //existingUser.Role.IsPrimary = false;
                        //_userDataService.Update(existingUser);
                        await base.RegisterUser(userViewModel);
                    }
                }

                response = Ok(Resource.UserEditedSuccessfully);

            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Route("DeleteUser")]
        [HttpPost]
        public async Task<IHttpActionResult> Delete([FromBody] UserViewModel userViewModel)
        {
            IHttpActionResult response = null;
            try
            {
                User user = (User)userViewModel;
                int result = _userDataService.Delete(user.ID);
                if (result == 1)
                {
                    response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, Resource.UserDeletedSuccessfully));
                }
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }


    }
}
