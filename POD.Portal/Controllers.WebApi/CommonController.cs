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
using System.IO;
using POD.Utilities;
using System.Web.Http.Results;

namespace POD.Portal.Controllers.WebApi
{
    public class CommonController : ApiController
    {
        protected CustomIdentityUserManager _userManager;
        public CustomIdentityUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<CustomIdentityUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }

        #region WEB API

        protected async Task<IHttpActionResult> RegisterUser(UserViewModel model)
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
                    UserIdentityInfo userinfo = UserHelper.GetUserInfo(this);
                    Guid organizationID = model.Organization.OrganizationID == default(Guid) ? userinfo.OrganizationID : model.Organization.OrganizationID;

                    Guid createdByID = userinfo.ID;
                    var user = new CustomIdentityUser(model.UserName, model.FullName, organizationID, model.Role.ID, createdByID, model.Role.IsPrimary);

                    var result = await UserManager.CreateAsync(user);
                    
                    if (result.Succeeded)
                    {
                        response = await SendPasswordResetEmail(user, true);
                        if (!(response is BadRequestErrorMessageResult))
                        response = Ok<object>(new object[2] { user.DBUser.ID, Resource.UserAddedSuccessfully });
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

        #endregion

        #region Private Methods

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("error", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        protected async Task<IHttpActionResult> SendPasswordResetEmail(CustomIdentityUser user, bool isRegistering)
        {
            IHttpActionResult response = null;
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed                
                response = Ok(Resource.EmailCheckMessage);
            }
            else
            {
                try
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    UriBuilder callbackUrl = new UriBuilder();
                    callbackUrl.Scheme = this.Url.Request.RequestUri.Scheme;
                    callbackUrl.Host = this.Url.Request.RequestUri.Host;
                    callbackUrl.Path = "Account/SetPassword";
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    query["userId"] = user.Id;
                    query["code"] = code;
                    callbackUrl.Query = query.ToString();

                    string body;
                    //return RedirectToAction("EmailConfirm");
                    //Read template file from the App_Data folder
                    if (isRegistering)
                    {
                        //using ( var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\App_Data\\Templates\\") + PODEnvironment.GetSetting("emailTemplate:SetPassword") ))
                        //{
                        //    body = sr.ReadToEnd();
                        //}
                        body = EmailManager.FillBody("SetPassword", new object[] { user.FullName, user.UserName, callbackUrl });
                        await UserManager.SendEmailAsync(user.Id, Resource.EmailTempSetPasswordSubject, string.Format(body, user.FullName, callbackUrl, user.UserName));
                    }
                    else
                    {

                        //using ( var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\App_Data\\Templates\\") + PODEnvironment.GetSetting("emailTemplate:ResetPassword") ))
                        //{
                        //    body = sr.ReadToEnd();
                        //}
                        body = EmailManager.FillBody("ResetPassword", new object[] { user.FullName, user.UserName, callbackUrl });
                        await UserManager.SendEmailAsync(user.Id, Resource.EmailTempForgotPasswordSubject, body);
                    }

                    response = Ok(Resource.EmailCheckMessage);

                }
                catch (Exception ex)
                {
                    string message = UserHelper.ErrorHandler(ex);
                    response = BadRequest(message);
                }
            }
            return response;
        }

        #endregion
    }
}