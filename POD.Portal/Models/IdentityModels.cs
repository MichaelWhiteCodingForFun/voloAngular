//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin;
//using POD.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;

//namespace POD.Portal.Models
//{
//    public class IdentityUserManager : UserManager<IdentityUser>
//    {
//        public IdentityUserManager(IUserStore<IdentityUser> store)
//            : base(store)
//        {
//        }

//        public static IdentityUserManager Create(IdentityFactoryOptions<IdentityUserManager> options,
//            IOwinContext context)
//        {
//            var manager = new IdentityUserManager(new MobizUserStore());

//            // Configure validation logic for usernames
//            manager.UserValidator = new UserValidator<IdentityUser>(manager)
//            {
//                AllowOnlyAlphanumericUserNames = false,
//                //RequireUniqueEmail = true
//            };

//            // Configure validation logic for passwords
//            manager.PasswordValidator = new PasswordValidator
//            {
//                RequiredLength = 6,
//                RequireNonLetterOrDigit = true,
//                RequireDigit = true,
//                RequireLowercase = true,
//                RequireUppercase = true,
//            };
//            // Configure user lockout defaults
//            manager.UserLockoutEnabledByDefault = bool.Parse(MobizEnvironment.GetSetting("UserLockoutEnabledByDefault"));
//            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(int.Parse(MobizEnvironment.GetSetting("DefaultAccountLockoutTimeSpan")));
//            manager.MaxFailedAccessAttemptsBeforeLockout = int.Parse(MobizEnvironment.GetSetting("MaxFailedAccessAttemptsBeforeLockout"));
//            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
//            // You can write your own provider and plug in here.
//            //manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<IdentityUser>
//            //{
//            //    MessageFormat = "Your security code is: {0}"
//            //});
//            //manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<IdentityUser>
//            //{
//            //    Subject = "SecurityCode",
//            //    BodyFormat = "Your security code is {0}"
//            //});

//            manager.EmailService = new EmailService();
//            //manager.SmsService = new SmsService();
//            var dataProtectionProvider = options.DataProtectionProvider;
//            if (dataProtectionProvider != null)
//            {
//                manager.UserTokenProvider =
//                    new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ResetPassword"));
//            }
//            return manager;
//        }

//        /// <summary>
//        ///     Create a user with no password.
//        ///     
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="confirmEmail">Indicate whether to confirm provided email</param>
//        /// <returns></returns>
//        public async Task<IdentityResult> CreateAsync(IdentityUser user, bool confirmEmail)
//        {
//            var result = await base.CreateAsync(user);
//            if (result != IdentityResult.Success)
//                return result;

//            if (confirmEmail)
//            {
//                var userStore = (MobizUserStore)Store;
//                await userStore.SetEmailConfirmedAsync(user, true);
//            }

//            return IdentityResult.Success;
//        }

//        public async Task<IdentityResult> CreateAsync(IdentityUser user, string password, bool confirmEmail)
//        {
//            var result = await base.CreateAsync(user, password);
//            if (result != IdentityResult.Success)
//                return result;

//            if (confirmEmail)
//            {
//                var userStore = (MobizUserStore)Store;
//                await userStore.SetEmailConfirmedAsync(user, true);
//            }

//            return IdentityResult.Success;
//        }

//        internal Task<IdentityResult> RegisterUser(Models.RegisterViewModel userModel)
//        {
//            throw new NotImplementedException();
//        }
//    }
//    /*
//        // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
//        public class ApplicationRoleManager : RoleManager<IdentityRole>
//        {
//            public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
//                : base(roleStore)
//            {
//            }

//            public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
//            {
//                return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
//            }
//        }
//            }
//        }
//     */

//    public class ApplicationSignInManager : SignInManager<IdentityUser, string>
//    {
//        public ApplicationSignInManager(IdentityUserManager userManager, IAuthenticationManager authenticationManager) :
//            base(userManager, authenticationManager)
//        { }

//        public override Task<ClaimsIdentity> CreateUserIdentityAsync(IdentityUser user)
//        {
//            return user.GenerateUserIdentityAsync((IdentityUserManager)UserManager);
//        }

//        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
//        {
//            return new ApplicationSignInManager(context.GetUserManager<IdentityUserManager>(), context.Authentication);
//        }
//    }

//    public class IdentityUser : IUser
//    {
//        User dbUser;

//        public IdentityUser()
//        {
//        }

//        public IdentityUser(User user)
//        {
//            this.dbUser = user;
//        }
//        public IdentityUser(string userName, string fullName)
//        {
//            this.dbUser = new User
//            {
//                //UserID = Guid.NewGuid(),
//                //UserName = userName,
//                //OrganizationID = fullName,
//                //Email = userName
//            };
//        }

//        public string Id
//        {
//            get { return this.dbUser.UserID.ToString(); }
//        }     

//        public string UserName
//        {
//            get
//            {
//                return this.dbUser.UserName;
//            }
//            set
//            {
//                this.dbUser.UserName = value;
//            }
//        }       

//        public User MobizUser
//        {
//            get
//            {
//                return this.dbUser;
//            }
//        }

//        public Task<ClaimsIdentity> GenerateUserIdentityAsync(IdentityUserManager manager)
//        {
//            return Task.FromResult(GenerateUserIdentity(manager));
//        }

//        public ClaimsIdentity GenerateUserIdentity(IdentityUserManager manager)
//        {
//            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
//            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
//            // Add custom user claims here
//            return userIdentity;
//        }
//    }
//}