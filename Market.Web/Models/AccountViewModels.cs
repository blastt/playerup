using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Market.Web.Attributes;

namespace Market.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Код")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Запомнить браузер?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Имя пользователя или почта")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]

        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]

        [Display(Name = "Имя пользователя")]
        //"^[-0-9A-Za-z_]{5,15}$"
        //Имя пользователя должно быть не менее 5 и не более 15 символов
        [StringLength(maximumLength: 15, MinimumLength = 5,ErrorMessage = "Имя пользователя должно быть не менее 5 и не более 15 символов")]
        [RegularExpression(pattern: "^[0-9A-Za-z]+$", ErrorMessage = "Имя пользователя не должно иметь символов и пробелов")]
        //[CustomValidation(typeof(UserNameValidation), "ValidateUserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }
    }

    public class ConfirmPhoneNumberViewModel
    {
        public bool IsConfirmed { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        public bool IsConfirmed { get; set; }

        public string Email { get; set; }
    }

    public class UpdateEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Новый адрес")]
        public string NewEmail { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public class UpdatePasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
    }

    public class UpdatePhoneNumberViewModel
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Новый номер")]
        public string PhoneNumber { get; set; }
    }
}
