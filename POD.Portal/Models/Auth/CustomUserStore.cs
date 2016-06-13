using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using POD.Data.Services;
using POD.Entities;
using POD.Entities.Enums;

namespace POD.Portal.Models.Auth
{
    //public class CustomUserSore<T> : IUserStore<T> where T : CustomCustomIdentityUser
    //{

    //    System.Threading.Tasks.Task IUserStore<T>.CreateAsync(T user)
    //    {
    //        //Create /Register New User 
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task IUserStore<T>.DeleteAsync(T user)
    //    {
    //        //Delete User 
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task<T> IUserStore<T>.FindByIdAsync(string userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task<T> IUserStore<T>.FindByNameAsync(string userName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task IUserStore<T>.UpdateAsync(T user)
    //    {
    //        //Update User Profile 
    //        throw new NotImplementedException();
    //    }

    //    void IDisposable.Dispose()
    //    {
    //        // throw new NotImplementedException(); 

    //    }
    //}

    public class CustomUserStore : IUserStore<CustomIdentityUser>
       , IUserLoginStore<CustomIdentityUser>
       , IUserPasswordStore<CustomIdentityUser>
       , IUserSecurityStampStore<CustomIdentityUser>
       //, IUserClaimStore<CustomIdentityUser>
       , IUserEmailStore<CustomIdentityUser>
       , IUserLockoutStore<CustomIdentityUser, string>
       , IUserTwoFactorStore<CustomIdentityUser, string>
    {
        public CustomUserStore()
        {

        }

        public void Dispose()
        {

        }

        #region IUserStore
        public virtual Task CreateAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForWrite())
                {
                    UserDataService.Instance.Create(user.DBUser);
                }
            });
        }

        public virtual Task DeleteAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForWrite())
                //{
                //    repository.DeleteUser(user.MobizUser.Id);
                //}
            });
        }

        public virtual Task<CustomIdentityUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            //Guid parsedUserId;
            //if (!Guid.TryParse(userId, out parsedUserId))
            //    throw new ArgumentException(string.Format("'{0}' is not a valid ID.", new { userId }),
            //        DataErrorCodes.InvalidUserId.ToString());

            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForRead())
                {
                    return new CustomIdentityUser((User)UserDataService.Instance.GetUserByID(Guid.Parse(userId)));
                }
            });
        }

        public virtual Task<CustomIdentityUser> FindByNameAsync(string userName)
        {
            //try
            //{

            //if (string.IsNullOrWhiteSpace(userName))
            //    throw new ArgumentNullException("userName");



            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForRead())
                {

                    var user = UserDataService.Instance.GetUserByUsername(userName);
                    return (user != null) ? new CustomIdentityUser(user) : null;
                }
            });
            //    }
            //catch(Exception ex)
            //    {

            //    }
        }

        public virtual Task UpdateAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForWrite())
                {
                    UserDataService.Instance.Update(user.DBUser);
                    // Update security information separately
                    AccountDataService.Instance.UpdateUserSecurity(user.DBUser);
                }
            });
        }
        #endregion

        #region IUserLoginStore
        public virtual Task AddLoginAsync(CustomIdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForWrite())
                //{
                //    UserExternalLogin uLogin = new UserExternalLogin
                //    {
                //        Id = Guid.NewGuid(),
                //        LoginProvider = login.LoginProvider,
                //        ProviderKey = login.ProviderKey,
                //        UserId = user.MobizUser.Id
                //    };
                //    repository.Insert(uLogin);
                //}
            });
        }

        public virtual Task<CustomIdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            return null;

            //return Task.Factory.StartNew(() =>
            //{
            //    using (var repository = MobizRepositoryFactory.GetForRead())
            //    {
            //        var userLogin = repository.GetUserLogin(login.LoginProvider, login.ProviderKey, true);
            //        if (userLogin == null)
            //            return null;

            //        return new CustomIdentityUser(userLogin.User);
            //    }
            //});
        }

        public virtual Task<IList<UserLoginInfo>> GetLoginsAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return null;

            //return Task.Factory.StartNew(() =>
            //{
            //    using (var repository = MobizRepositoryFactory.GetForRead())
            //    {
            //        IList<UserExternalLogin> dbLogins = repository.GetAllUserLogin(user.MobizUser.Id);
            //        IList<UserLoginInfo> idLogins = new List<UserLoginInfo>();
            //        foreach (var login in dbLogins)
            //        {
            //            idLogins.Add(
            //                new UserLoginInfo(login.LoginProvider, login.ProviderKey)
            //                );
            //        }
            //        return idLogins;
            //    }
            //});
        }

        public virtual Task RemoveLoginAsync(CustomIdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            return null;

            //return Task.Factory.StartNew(() =>
            //{
            //    using (var repository = MobizRepositoryFactory.GetForWrite())
            //    {
            //        //TODO:OPTIMIZE
            //        //TODO:VERIFY USER MATCHING
            //        var dbLogin = repository.GetUserLogin(login.LoginProvider, login.ProviderKey);
            //        repository.DeleteUserLogin(dbLogin.Id);
            //    }
            //});
        }
        #endregion

        #region IUserPasswordStore
        public virtual Task<string> GetPasswordHashAsync(CustomIdentityUser user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException("user");

                return Task.FromResult(user.DBUser.PasswordHash);
            }
            catch (Exception ex)
            { return null; }
        }

        public virtual Task<bool> HasPasswordAsync(CustomIdentityUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.DBUser.PasswordHash));
        }

        public virtual Task SetPasswordHashAsync(CustomIdentityUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.DBUser.PasswordHash = passwordHash;

            //unlocking the user in any case.
            user.DBUser.LockoutEndDateUtc = null;
            user.DBUser.StatusID = (short)AccountStatus.Active;

            return Task.FromResult(0);
        }

        #endregion

        #region IUserSecurityStampStore
        public virtual Task<string> GetSecurityStampAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.DBUser.SecurityStamp);
        }

        public virtual Task SetSecurityStampAsync(CustomIdentityUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.DBUser.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        #endregion

        #region IUserClaimStore

        //public Task AddClaimAsync(CustomIdentityUser user, System.Security.Claims.Claim claim)
        //{
        //    using (var repository = MobizRepositoryFactory.GetForWrite() as ISecurityRepository)
        //    {
        //        IList<UserClaim> claims = repository.GetAllUserClaims(user.MobizUser.Id, claim.Type);
        //        if (!claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
        //        {
        //            repository.Insert(new UserClaim
        //            {
        //                UserId = user.MobizUser.Id,
        //                ClaimType = claim.Type,
        //                ClaimValue = claim.Value
        //            });
        //        }

        //        return Task.FromResult(0);
        //    }

        //   // return null;
        //}

        //public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(CustomIdentityUser user)
        //{
        //    //using (var repository = MobizRepositoryFactory.GetForRead() as ISecurityRepository)
        //    //{
        //    //    IList<UserClaim> claims = repository.GetAllUserClaims(user.MobizUser.Id);
        //    //    IList<System.Security.Claims.Claim> results = claims.Select(c => new
        //    //        System.Security.Claims.Claim(c.ClaimType, c.ClaimValue)).ToList();
        //    //    return Task.FromResult(results);

        //    //}

        //    return null;
        //}

        //public Task RemoveClaimAsync(CustomIdentityUser user, System.Security.Claims.Claim claim)
        //{
        //    //using (var repository = MobizRepositoryFactory.GetForWrite() as ISecurityRepository)
        //    //{
        //    //    repository.DeleteUserClaim(user.MobizUser.Id, claim.Type, claim.Value);
        //    //    return Task.FromResult(0);
        //    //}

        //    return null;
        //}

        #endregion

        #region IUserEmailStore
        public Task<CustomIdentityUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");

            return Task.Factory.StartNew(() =>
            {
                // using (var repository = MobizRepositoryFactory.GetForRead())
                {
                    var user = UserDataService.Instance.GetUserByUsername(email);
                    return (user != null) ? new CustomIdentityUser(user) : null;
                }
            });
        }

        public Task<string> GetEmailAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                //using (var repository = MobizRepositoryFactory.GetForRead())
                {

                    var userEmail = UserDataService.Instance.GetUserByID(Guid.Parse(user.Id)).UserName;
                    return (userEmail != null) ? userEmail : null;
                }
            });

        }

        public Task<bool> GetEmailConfirmedAsync(CustomIdentityUser user)
        {
            //return Task.Factory.StartNew(() =>
            //{
            //    // using (var repository = MobizRepositoryFactory.GetForRead())
            //    {
            //        User userData = UserDataService.Instance.GetUserByID(Guid.Parse(user.Id));

            //        return userData.EmailConfirmed;
            //    }
            //});
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(CustomIdentityUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(CustomIdentityUser user, bool confirmed)
        {
            //return Task.Factory.StartNew(() =>
            //{
            //    // using (var repository = AccountDataService.Instance.))

            //    {
            //        AccountDataService.Instance.VerifyUserEmail(Guid.Parse(user.Id), user.UserName);
            //    }
            //});

            throw new NotImplementedException();

        }

        #endregion

        #region IUserLockoutStore
        public Task<int> GetAccessFailedCountAsync(CustomIdentityUser user)
        {
            return Task.FromResult(user.DBUser.AccessFailedCount); ;
        }

        public Task<bool> GetLockoutEnabledAsync(CustomIdentityUser user)
        {
            return Task.FromResult(user.DBUser.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(CustomIdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            //return Task.FromResult(default(DateTimeOffset));

            return
                Task.FromResult(user.DBUser.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.DBUser.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task<int> IncrementAccessFailedCountAsync(CustomIdentityUser user)
        {
            return Task.FromResult(++user.DBUser.AccessFailedCount);
            //if(user.DBUser.AccessFailedCount == 2)
            //    user.DBUser.LockoutSequence = 0;
        }

        public Task ResetAccessFailedCountAsync(CustomIdentityUser user)
        {
            user.DBUser.AccessFailedCount = 0;

            //user.DBUser.ForcedPasswordReset = false;
            //user.DBUser.LockoutSequence = 0;
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(CustomIdentityUser user, bool enabled)
        {
            user.DBUser.LockoutEnabled = enabled;
            return Task.FromResult<bool>(false);
        }

        public Task SetLockoutEndDateAsync(CustomIdentityUser user, DateTimeOffset lockoutEnd)
        {
            user.DBUser.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            //set infinite lock
            user.DBUser.StatusID = (short)AccountStatus.Locked;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(CustomIdentityUser user)
        {
            //return Task.FromResult(user.DBUser.TwoFactorEnabled);
            return Task.FromResult(false);
        }

        public Task SetTwoFactorEnabledAsync(CustomIdentityUser user, bool enabled)
        {
            //user.DBUser.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }
        #endregion
    }
}