using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Marketplace.Data;
using Market.Model.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Configuration;

namespace Market.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            
            return ConfigSendGridasync(message);
        }

        private async Task ConfigSendGridasync(IdentityMessage message)
        {
            try
            {
                // Create a Web transport for sending email.
                var apiKey = ConfigurationManager.AppSettings["SENDGRID_KEY"];
                var client = new SendGridClient(apiKey);
                var to = new EmailAddress(message.Destination);
                var subject = message.Subject;

                var from = new EmailAddress("support@playerup.ru", "PlayerUp"); 
                var plainTextContent = message.Body;
                var htmlContent = message.Body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception)
            {

               
            }
            
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var ACCOUNT_SID = ConfigurationManager.AppSettings["ACCOUNT_SID"];
            var AUTH_TOKEN = ConfigurationManager.AppSettings["AUTH_TOKEN"];

            TwilioClient.Init(ACCOUNT_SID, AUTH_TOKEN);

            var msg = await MessageResource.CreateAsync(
                to: new PhoneNumber(message.Destination),
                from: new PhoneNumber("+15172101708"),
                body: message.Body
                );
            

        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<MarketEntities>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Код, полученный по телефону", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Ваш код безопасности: {0}"
            });
            manager.RegisterTwoFactorProvider("Код из сообщения", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Код безопасности",
                BodyFormat = "Ваш код безопасности: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
