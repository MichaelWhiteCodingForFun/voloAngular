using Microsoft.AspNet.Identity;
using POD.Entities;
using POD.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace POD.Portal.Models.Auth
{
    public class CustomIdentityUser : IUser
    {
        private User dbUser;

        //private string userName = string.Empty;
        //public string Email = string.Empty;
        //public int OrganizationID = 0;
        ////private string id = string.Empty;
        //public int RoleID = 0;

        public string Id { get { return this.dbUser.ID.ToString(); } }


        public string UserName
        {
            get { return this.dbUser.UserName; }
            set
            {
                this.dbUser.UserName = value;
            }
        }
        public string FullName
        {
            get { return this.dbUser.FullName; }
        }

        public CustomIdentityUser(string userName, string fullName, Guid organizationID, Guid roleID, Guid createdByID, bool isPrimary)
        {
            this.dbUser = new User
            {
                ID = Guid.NewGuid(),
                UserName = userName,
                FullName = fullName,
                //EmailConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                CreatedDateUtc = DateTime.Now,
                IsActive = true,
                Role = new UserRole() { RoleID = roleID, IsPrimary = isPrimary},
                Organization = new UserOrganization() { OrganizationID = organizationID },
                PasswordHash = String.Empty,
                LockoutEndDateUtc = null,
                StatusID = (short)AccountStatus.Inactive,
                SecurityStamp = String.Empty,
                CreatedByID = createdByID,
                LastLoginDate = null,


                // OrganizationCode = organizationCode
            };
        }
        public CustomIdentityUser(User user)
        {
            this.dbUser = user;
        }

        public User DBUser
        {
            get
            {
                return this.dbUser;
            }
        }


        //public Task<ClaimsIdentity> GenerateUserIdentityAsync(CustomIdentityUserManager manager)
        //{
        //    return Task.FromResult(GenerateUserIdentity(manager));
        //}

        //public ClaimsIdentity GenerateUserIdentity(CustomIdentityUserManager manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    userIdentity.AddClaim(new System.Security.Claims.Claim("RoleID", this.DBUser.Role.ID.ToString()));
        //    // Add custom user claims here
        //    return userIdentity;
        //}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(CustomIdentityUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            string roleDisplayName = String.Format("{0} {1}", (OrganizationType)this.DBUser.Organization.OrganizationTypeID, this.DBUser.Role.RoleName);

            userIdentity.AddClaim(new System.Security.Claims.Claim("FullName", this.DBUser.FullName));
            userIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role, roleDisplayName));
            userIdentity.AddClaim(new System.Security.Claims.Claim("RoleID", this.DBUser.Role.RoleID.ToString()));
            userIdentity.AddClaim(new System.Security.Claims.Claim("IsAdmin", this.DBUser.Role.IsAdmin.ToString()));
            userIdentity.AddClaim(new System.Security.Claims.Claim("OrganizationID", this.DBUser.Organization.OrganizationID.ToString()));
            userIdentity.AddClaim(new System.Security.Claims.Claim("OrganizationTypeID", this.DBUser.Organization.OrganizationTypeID.ToString()));
            userIdentity.AddClaim(new System.Security.Claims.Claim("OrganizationName", this.DBUser.Organization.OrganizationName));

            return userIdentity;
        }

    }
}