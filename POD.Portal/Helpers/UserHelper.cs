using POD.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using POD.Portal.Models.Auth;

namespace POD.Portal.Helpers
{
    public static class UserHelper
    {
        public static UserIdentityInfo GetUserInfo(ApiController controllerContext)
        {
            UserIdentityInfo userInfo = new UserIdentityInfo();

            userInfo.ID = Guid.Parse(controllerContext.User.Identity.GetUserId());

            var claimsIdentity = controllerContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                Claim nameClaim = claimsIdentity.FindFirst("FullName");
                if (nameClaim != null)
                    userInfo.FullName = nameClaim.Value;

                Claim roleClaim = claimsIdentity.FindFirst(ClaimTypes.Role);
                if (roleClaim != null)
                    userInfo.Role = roleClaim.Value;

                Claim organizationIDClaim = claimsIdentity.FindFirst("OrganizationID");
                if (organizationIDClaim != null)
                    userInfo.OrganizationID = Guid.Parse(organizationIDClaim.Value);

                Claim roleIDClaim = claimsIdentity.FindFirst("RoleID");
                if (roleIDClaim!=null)
                {
                    userInfo.RoleID = Guid.Parse(roleIDClaim.Value);
                }

                Claim isAdminClaim = claimsIdentity.FindFirst("IsAdmin");
                if (isAdminClaim != null)
                {
                    userInfo.IsAdmin = Boolean.Parse(isAdminClaim.Value);
                }

                Claim organizationTypeIDClaim = claimsIdentity.FindFirst("OrganizationTypeID");
                if (organizationTypeIDClaim != null)
                {
                    userInfo.OrganizationTypeID = Int32.Parse(organizationTypeIDClaim.Value);
                }

                Claim organizationNameClaim = claimsIdentity.FindFirst("OrganizationName");
                if (organizationNameClaim != null)
                {
                    userInfo.OrganizationName = organizationNameClaim.Value;
                }

            }

            return userInfo;
        }

        public static string ErrorHandler(Exception e)
        {
            if (e is SqlException)
            {
                return "Database error";
            }
            else
            {
                return e.Message;
            }
        }

        //public static CustomIdentityUser CreateCustomIdentityUser(ApiController controllerContext, UserViewModel model)
        //{
        //    //TODO
        //    Guid organizationID = model.Role.OrganizationID;
        //    UserIdentityInfo userinfo = GetUserInfo(controllerContext);


        //    ////organization is current logged in
        //    //if (model.Organization == null)
        //    //{
        //    //    //current or logged-in user
        //    //    organizationID = userinfo.OrganizationID;
        //    //}
        //    //else
        //    //{
        //    //    //organization passed with the user
        //    //    organizationID = model.Organization.OrganizationID;
        //    //}

        //    Guid createdByID = userinfo.ID;
        //    var user = new CustomIdentityUser(model.UserName, model.FullName, organizationID, model.Role.ID, createdByID);

        //    return user;       
        //}

      

    }
}