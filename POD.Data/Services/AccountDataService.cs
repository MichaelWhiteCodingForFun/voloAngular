using System;
using System.Linq;
using POD.Data.Dapper;
using POD.Entities;
using POD.Interfaces;

namespace POD.Data.Services
{
    public class AccountDataService : BaseDataService, IAccountDataService
    {
        static AccountDataService _instance;
        //public AccountDataService(PODContext context) : base(context)
        //{
        //    _instance = this;
        //}

        public AccountDataService()
        {
            _instance = this;
        }

        public static AccountDataService Instance
        {
            get { return _instance == null? new AccountDataService() : _instance; }
        }
     

        //public void VerifyUserEmail(Guid userID, string email)
        //{
        //    var res = this.GetList<User>(new { ID = userID, UserName = email });
        //    if (res.Count() == 0)
        //    {
        //        throw new ArgumentException(string.Format("Couldn't verify User Email.  User not found: {0}", userID), /*DataErrorCodes.NoUser.ToString()*/ "2");
        //    }
        //    else if (res.Count() > 1)
        //    {
        //        // throw new InvalidOperationException("Get by username returned more than 1 user for username: " + username);
        //    }

        //    User user = res.Single();

     
        //    base.Update(user);
        //}

        public void UpdateUserSecurity(User user)
        {
            try
            {
                string sql = @"UPDATE ""user"" SET
                AccessFailedCount = @AccessFailedCount,
                LockoutEnabled = @LockoutEnabled,
                LockoutEndDateUtc = @LockoutEndDateUtc,
                StatusID = @StatusID,
                PasswordHash = @PasswordHash,
                SecurityStamp = @SecurityStamp,
                LastLoginDate = @LastLoginDate    
                WHERE Id = @Id";
                base.Execute(sql, new
                {
                    AccessFailedCount = user.AccessFailedCount,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutEndDateUtc = user.LockoutEndDateUtc,
                    StatusID = user.StatusID,
                    PasswordHash = user.PasswordHash,
                    SecurityStamp = user.SecurityStamp,
                    LastLoginDate = user.LastLoginDate,
                    ID = user.ID
                });

                //Update<User>(new User() { AccessFailedCount = user.AccessFailedCount,
                //    LockoutEnabled = user.LockoutEnabled, LockoutEndDateUtc = user.LockoutEndDateUtc, PasswordHash = user.PasswordHash,
                //    SecurityStamp = user.SecurityStamp
                //});
            }
            catch (Exception ex)
            {
                _loggingManager.Log.Error(ex, ex.Source);
                throw;
            }
        }
    }
}
