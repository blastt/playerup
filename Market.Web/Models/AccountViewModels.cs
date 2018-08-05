using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
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
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

        [Required(ErrorMessage = "Введите имя пользователя или почту")]
        [Display(Name = "Имя пользователя или почта")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите имя пользователя")]

        [Display(Name = "Имя пользователя")]
        //"^[-0-9A-Za-z_]{5,15}$"
        //Имя пользователя должно быть не менее 5 и не более 15 символов
        [StringLength(maximumLength: 15, MinimumLength = 5,ErrorMessage = "Имя пользователя должно быть не менее 5 и не более 15 символов")]
        [RegularExpression(pattern: "^[0-9A-Za-z]+$", ErrorMessage = "Имя пользователя не должно иметь специальных символов и состоять из букв кирилицы (a-z)")]
        //[CustomValidation(typeof(UserNameValidation), "ValidateUserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        //[Required]
        //[EmailAddress]
        //[Display(Name = "Адрес электронной почты")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class RestorePasswordViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }



    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
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
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Новый адрес")]
        public string NewEmail { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }

    public class UpdatePasswordViewModel
    {
        [Required(ErrorMessage = "Введите текущий пароль")]        
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} и не более {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }
    }

    public class UpdatePhoneNumberViewModel
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Новый номер")]
        public string PhoneNumber { get; set; }
    }
}
