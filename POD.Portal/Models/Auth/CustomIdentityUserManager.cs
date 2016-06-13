using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using POD.Data.Dapper;
using POD.Portal.Models.Auth.Services;
using POD.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace POD.Portal.Models.Auth
{
    public class CustomIdentityUserManager : UserManager<CustomIdentityUser>
    {

        public CustomIdentityUserManager(IUserStore<CustomIdentityUser> store)
            : base(store)
        {
        }
        public static CustomIdentityUserManager Create(IdentityFactoryOptions<CustomIdentityUserManager> options,
              IOwinContext context)
        {
            var manager = new CustomIdentityUserManager(new CustomUserStore());

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<CustomIdentityUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = bool.Parse(PODEnvironment.GetSetting("UserLockoutEnabledByDefault"));
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(int.Parse(PODEnvironment.GetSetting("DefaultAccountLockoutTimeSpan")));
            manager.MaxFailedAccessAttemptsBeforeLockout = int.Parse(PODEnvironment.GetSetting("MaxFailedAccessAttemptsBeforeLockout"));
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            //manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<IdentityUser>
            //{
            //    MessageFormat = "Your security code is: {0}"
            //});
            //manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<IdentityUser>
            //{
            //    Subject = "SecurityCode",
            //    BodyFormat = "Your security code is {0}"
            //});

            manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<CustomIdentityUser>(dataProtectionProvider.Create("ResetPassword"));
            }
            return manager;
        }

        /// <summary>
        ///     Create a user with no password.
        ///     
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmEmail">Indicate whether to confirm provided email</param>
        /// <returns></returns>
        //public async Task<IdentityResult> CreateAsync(CustomIdentityUser user, bool confirmEmail)
        //{
        //    var result = await base.CreateAsync(user);
        //    if (result != IdentityResult.Success)
        //        return result;

        //    if (confirmEmail)
        //    {
        //        var userStore = (CustomUserStore)Store;
        //        await userStore.SetEmailConfirmedAsync(user, true);
        //    }

        //    return IdentityResult.Success;
        //}

        public async Task<IdentityResult> CreateAsync(CustomIdentityUser user/*, string password, bool confirmEmail*/)
        {
            try
            {
                var result = await base.CreateAsync(user);
                if (result != IdentityResult.Success)
                    return result;

                //if (confirmEmail)
                //{
                //    var userStore = (CustomUserStore)Store;
                //    await userStore.SetEmailConfirmedAsync(user, true);
                //}
            }
            catch(Exception ex)
            {

            }

            return IdentityResult.Success;
        }

        //internal Task<IdentityResult> RegisterUser(Models.RegisterViewModel userModel)
        //{
        //    throw new NotImplementedException();
        //}
    }

    /// <summary> 
    /// Use Custom approach to verify password 
    /// </summary> 
    public class OldSystemPasswordHasher : PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return base.HashPassword(password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {

            //Here we will place the code of password hashing that is there in our current solucion.This will take cleartext anad hash 
            //Just for demonstration purpose I always return true.     
            if (true)
            {


                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}