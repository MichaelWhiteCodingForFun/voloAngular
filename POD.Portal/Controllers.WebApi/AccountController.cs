using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Ninject;
using POD.Entities;
using POD.Interfaces;
using POD.Portal.Models;
using POD.Portal.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using POD.Portal;
using System.Collections.ObjectModel;
using POD.Portal.Helpers;
using POD.Entities.Enums;

namespace POD.Portal.Controllers.WebApi
{
    [RoutePrefix("api/accounts")]
    public class AccountController : CommonController
    {
        [Inject]
        public IAccountDataService _accountDataService { get; set; }

        [Inject]
        public IUserDataService _userDataService { get; set; }

        [Inject]
        public IReportDataService _reportDataService { get; set; }

        public AccountController(IAccountDataService accountDataService, IUserDataService userDataService)
        {
            _accountDataService = accountDataService;
            _userDataService = userDataService;
        }

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }


        #region WEB API

        [AllowAnonymous]
        [Route("Login")]            
        [HttpPost]
        public async Task<IHttpActionResult> Login(HttpRequestMessage request, [FromBody] LoginViewModel model)
        {
            IHttpActionResult response = null;
            try
            {
                CustomIdentityUser user = await UserManager.FindByNameAsync(model.UserName);
                //if (user != null && user.DBUser.LockoutEndDateUtc != null)
                if(user != null)
                {
                    switch (user.DBUser.StatusID)
                    {
                        case (short)AccountStatus.Inactive:
                            ModelState.AddModelError("error", Resource.AccountInactive);
                            return BadRequest(ModelState);
                            break;
                        case (short)AccountStatus.Locked:
                            ModelState.AddModelError("error", Resource.AccountLockout);
                            return BadRequest(ModelState);
                            break;
                        case (short)AccountStatus.Disabled:
                            ModelState.AddModelError("error", Resource.AccountDisabled);
                            return BadRequest(ModelState);
                            break;
                    }
                }

                //if (user != null && user.DBUser.StatusID == (short)AccountStatus.Locked)
                //{
                //    ModelState.AddModelError("error", Resource.AccontLockout);
                //    return BadRequest(ModelState);
                //}

                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);

                if (result == SignInStatus.Success)
                {
                    user = await UserManager.FindByNameAsync(model.UserName);
                    user.DBUser.LastLoginDate = DateTime.Now;
                    await UserManager.UpdateAsync(user);

                    Report ReportEntity = new Entities.Report()
                    {
                        UserID = user.DBUser.ID,
                        OrganizationID = user.DBUser.Organization.OrganizationID,
                        ReportTypeID = (int)ReportType.Login,
                        Details = "LOGIN",
                        IsSuccess = true,
                        TicketID = null,
                        CreatedDateUtc = DateTime.UtcNow
                    };                    

                    //Guid reportID = await _reportDataService.AddReportAsync(ReportEntity, false);
                    //
                }

                IHttpActionResult errorResult = GetErrorResult(result);
                if (errorResult != null)
                {
                    return errorResult;
                }
                else
                    response = Ok();
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }

            return response;

        }

        [HttpGet]
        [Route("LogOut")]
        public IHttpActionResult LogOut()
        {
            IHttpActionResult response = null;
            try
            {
                AuthenticationManager.SignOut();
                response = Ok();
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        // POST api/Accounts/Register

        [Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(UserViewModel model)
        {
            return await base.RegisterUser(model);
        }


        //[Authorize(Roles = "System Admin, Company Admin, Customer Admin")]
        //[Route("addMultiple")]
        //[HttpPost]
        //public async Task<IHttpActionResult> MultiRegister(List<UserViewModel> models)
        //{
        //    return await base.MultiRegisterUser(models);
            
        //}

        //[AllowAnonymous]
        //[Route("ConfirmEmail")]
        //[HttpPost]
        //public async Task<IHttpActionResult> ConfirmEmail(ConfirmEmailByCodeViewModel model)
        //{
        //    var result = await UserManager.ConfirmEmailAsync(model.UserID, model.Code);
        //    if (!result.Succeeded)
        //        BadRequest();
        //    return Ok();
        //}



        [AllowAnonymous]
        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            IHttpActionResult response = null;
            try
            {
                if (!ModelState.IsValid)
                {
                    response = BadRequest(ModelState);
                }
                else
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user != null)
                    {
                        switch (user.DBUser.StatusID)
                        {
                            case (short)AccountStatus.Disabled:
                                ModelState.AddModelError("error", Resource.AccountDisabled);
                                return BadRequest(ModelState);
                                break;
                        }
                    }               
                    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        response = Ok(Resource.ResetPasswordSuccess);
                    }
                    else
                    {
                        IHttpActionResult errorResult = GetErrorResult(result);

                        if (errorResult != null)
                        {
                            response = errorResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = UserHelper.ErrorHandler(ex);
                response = BadRequest(message);
            }
            return response;
        }

        [Route("ForgotPassword")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            IHttpActionResult response = null;
            if (!ModelState.IsValid)
            {
                response = BadRequest(ModelState);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    switch (user.DBUser.StatusID)
                    {
                        case (short)AccountStatus.Disabled:
                            ModelState.AddModelError("error", Resource.AccountDisabled);
                            return BadRequest(ModelState);
                            break;
                    }
                }
                else
                {
                    // Don't reveal that the user does not exist
                }           

                response = await SendPasswordResetEmail(user, false);
            }

            return response;
        }

        #endregion

        #region Private Methods

        //private IHttpActionResult GetErrorResult(IdentityResult result)
        //{
        //    if (result == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (!result.Succeeded)
        //    {
        //        if (result.Errors != null)
        //        {
        //            foreach (string error in result.Errors)
        //            {
        //                ModelState.AddModelError("error", error);
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // No ModelState errors are available to send, so just return an empty BadRequest.
        //            return BadRequest();
        //        }

        //        return BadRequest(ModelState);
        //    }

        //    return null;
        //}

        private IHttpActionResult GetErrorResult(SignInStatus result)
        {
            if (result == null)
            {
                return InternalServerError();
            }
            switch (result)
            {
                case SignInStatus.Success:
                    return null;
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("error", Resource.AccountLockout);
                    return BadRequest(ModelState);
                case SignInStatus.RequiresVerification:
                    ModelState.AddModelError("error", result.ToString());
                    return BadRequest(ModelState);
                case SignInStatus.Failure:
                    ModelState.AddModelError("error", Resource.InncorectCredentials);
                    return BadRequest(ModelState);
                default:
                    ModelState.AddModelError("error", "Error");
                    return Unauthorized();
            }
        }

        private UserIdentityInfo GetUserInfo()
        {
            UserIdentityInfo userInfo = new UserIdentityInfo();
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                Claim roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
                if (roleClaim != null)
                    userInfo.Role = roleClaim.Value;

                Claim customerClaim = claimsIdentity.FindFirst("OrganizationID");
                if (customerClaim != null)
                    userInfo.OrganizationID = Guid.Parse(customerClaim.Value);
            }

            return userInfo;
        }

        //private async Task SendConfirmationEmail(CustomIdentityUser user)
        //{
        //    if (user == null)
        //        return;
        //    try
        //    {
        //        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //        UriBuilder callbackUrl = new UriBuilder();
        //        callbackUrl.Scheme = this.Url.Request.RequestUri.Scheme;
        //        callbackUrl.Host = this.Url.Request.RequestUri.Host;
        //        callbackUrl.Path = "Account/ConfirmEmail";
        //        var query = HttpUtility.ParseQueryString(string.Empty);
        //        query["userId"] = user.Id;
        //        query["code"] = code;
        //        callbackUrl.Query = query.ToString();
        //        await UserManager.SendEmailAsync(user.Id,
        //           Resource.EmailTempConfirmEmailSubject,
        //           string.Format(Resource.EmailTempConfirmEmailBody, callbackUrl));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion
    }
}