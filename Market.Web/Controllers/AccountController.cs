using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Market.Web.Models;
using Market.Model.Models;
using Market.Service;
using static Market.Web.Controllers.ManageController;

namespace Market.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly IUserProfileService _userProfileService;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        #region Custom Methods

        [Authorize]
        public ActionResult Settings()
        {
            return View();
        }
        public PartialViewResult AccountMenu()
        {
            return PartialView("_AccountMenu");
        }
        public ActionResult ControlPanel()
        {
            return View();
        }


        [Authorize]
        public async Task<ActionResult> VerifyEmail()
        {
            if (User != null)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                
                // get user object from the storage
                

                    // change username and email

                    // Persiste the changes
                // generage email confirmation code
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                await UserManager.SendEmailAsync(user.Id, "Подтверждение почты", "Для почтверждения почты" +
                    " учётной записи Для смены адреса вам необходимо его активировать, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");

                return View("VerifyEmailRequest");
            }
            return View("ChangeEmailError");

            // send email to the user with the confirmation link
        }
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("ChangeEmailError");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "ChangeEmailError");
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateEmail(UpdateEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                // get user object from the storage
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                var result = await UserManager.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    // change username and email
                    
                    // Persiste the changes

                    bool emailExists = false;
                    foreach (var u in UserManager.Users)
                    {
                        if (u.Email == model.NewEmail && u.EmailConfirmed)
                        {
                            emailExists = true;

                            break;
                        }
                    }
                    if (emailExists)
                    {
                        return View("ChangeEmailError");
                    }
                    user.Email = model.NewEmail;
                    user.EmailConfirmed = false;
                    await UserManager.UpdateAsync(user);
                        // generage email confirmation code

                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                        await UserManager.SendEmailAsync(user.Id, "Подтверждение почты", "Вы предоставили новый email-адрес для вашей" +
                            " учётной записи Для смены адреса вам необходимо его активировать, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");

                        return View("UpdateEmailRequest");
                    
                }
                return View("ChangeEmailError");
            }
            return View("ChangeEmailError");

            // send email to the user with the confirmation link
        }

        [Authorize]
        public ActionResult UpdateEmail()
        {
            return View();
        }
        

        [Authorize]
        public async Task<ActionResult> ChangeEmail(string userId, string code)
        {

            if (userId == null || code == null)
            {
                return View("ChangeEmailError");
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                //await UserManager.SetEmailAsync(userId, newEmail);
                return View("UpdateEmailSuccess");
            }
            return View("ChangeEmailError");
        }

        [Authorize]
        public ActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user == null)
                {
                    // Не показывать, что пользователь не существует или не подтвержден
                    return View("ChangePasswordError");
                }

                {
                    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangePasswordConfirmation", "Account");
                    }
                    // Дополнительные сведения о включении подтверждения учетной записи и сброса пароля см. на странице https://go.microsoft.com/fwlink/?LinkID=320771.
                    // Отправка сообщения электронной почты с этой ссылкой

                    return View("ChangePasswordError");
                }

            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View("ChangePasswordError");
        }

        [Authorize]
        public ActionResult ChangePasswordConfirmation()
        {
            return View("ChangePasswordSuccess");
        }

        #endregion


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true
            var userName = model.UserNameOrEmail;
            
            var user = _userProfileService.GetUserProfiles().FirstOrDefault(m => model.UserNameOrEmail == m.ApplicationUser.Email);
            if (user != null)
            {
                userName = user.Name;
            }
            
            var result = await SignInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Требовать предварительный вход пользователя с помощью имени пользователя и пароля или внешнего имени входа
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Приведенный ниже код защищает от атак методом подбора, направленных на двухфакторные коды. 
            // Если пользователь введет неправильные коды за указанное время, его учетная запись 
            // будет заблокирована на заданный период. 
            // Параметры блокирования учетных записей можно настроить в IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неправильный код.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(_userProfileService.GetUserProfileByName(model.UserName) != null)
                {
                    return HttpNotFound("User exists!");
                }
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                string urlPath = Url.Content("~/Content/Images/Avatars/Default.png");
                UserProfile profile = new UserProfile
                {
                    Id = user.Id,
                    ImagePath = urlPath,
                    Name = user.UserName,
                    RegistrationDate = DateTime.Now,
                    ApplicationUser = user
                };
                _userProfileService.CreateUserProfile(profile);
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // Дополнительные сведения о включении подтверждения учетной записи и сброса пароля см. на странице https://go.microsoft.com/fwlink/?LinkID=320771.
                    // Отправка сообщения электронной почты с этой ссылкой
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Подтверждение учетной записи", "Подтвердите вашу учетную запись, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");
                    


                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        public PartialViewResult ConfirmEmailPartial()
        {
            var currentUserId = User.Identity.GetUserId();
            var isConfirmed = UserManager.IsEmailConfirmed(currentUserId);
            var email = UserManager.GetEmail(currentUserId);
            ConfirmEmailViewModel model = new ConfirmEmailViewModel()
            {
                IsConfirmed = isConfirmed,
                Email = email
            };
            return PartialView("_ConfirmEmail",model);
        }

        public PartialViewResult ConfirmPhoneNumberPartial()
        {
            var currentUserId = User.Identity.GetUserId();
            var isConfirmed = UserManager.IsPhoneNumberConfirmed(currentUserId);
            var phoneNumber = UserManager.GetPhoneNumber(currentUserId);
            ConfirmPhoneNumberViewModel model = new ConfirmPhoneNumberViewModel()
            {
                IsConfirmed = isConfirmed,
                 PhoneNumber = phoneNumber
            };
            return PartialView("_ConfirmPhoneNumber", model);
        }

        //
        // GET: /Account/ConfirmEmail
        

        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool numberExists = false;
            foreach (var user in UserManager.Users)
            {
                if (user.PhoneNumber == model.Number)
                {
                    numberExists = true;
                    break;
                }
            }
            if (numberExists)
            {
                return HttpNotFound("Такой номер уже существует");
            }
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(currentUserId, model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            return phoneNumber == null ? View("UpdatePhoneNumberError") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber, Code = code });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                return View("UpdatePhoneNumberSuccess");
                //return RedirectToAction("Settings", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            ModelState.AddModelError("", "Failed to verify phone");
            return View("UpdatePhoneNumberError");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePhoneNumber(UpdatePhoneNumberViewModel model)
        {
            var currentUserId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            bool numberExists = false;
            
            foreach (var user in UserManager.Users)
            {
                if (user.Id == currentUserId)
                {
                    continue;
                }
                if (user.PhoneNumber == model.PhoneNumber)
                {
                    numberExists = true;
                    break;
                }
            }
            if (numberExists)
            {
                return View("UpdatePhoneNumberError");
            }
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(currentUserId, model.PhoneNumber);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.PhoneNumber,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.PhoneNumber });
        }

     
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Не показывать, что пользователь не существует или не подтвержден
                    return View("ForgotPasswordConfirmation");
                }

                // Дополнительные сведения о включении подтверждения учетной записи и сброса пароля см. на странице https://go.microsoft.com/fwlink/?LinkID=320771.
                // Отправка сообщения электронной почты с этой ссылкой
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Сброс пароля", "Сбросьте ваш пароль, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View("ChangePasswordError");
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("ChangePasswordError") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Не показывать, что пользователь не существует
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View("ChangePasswordError");
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Запрос перенаправления к внешнему поставщику входа
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Создание и отправка маркера
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Выполнение входа пользователя посредством данного внешнего поставщика входа, если у пользователя уже есть имя входа
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Если у пользователя нет учетной записи, то ему предлагается создать ее
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Получение сведений о пользователе от внешнего поставщика входа
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }
            
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}