using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eStore.Models
{
    public class UsersViewModel
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "حالة الحساب")]
        public bool IsActive { get; set; }
        [Display(Name = "صلاحية الحساب")]
        public string UserType { get; set; }
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "البريد الإلكتروني")]
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
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        public string Provider { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "الكود")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "تذكرني على هذا المتصفح؟")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required(ErrorMessage = "{0} حقل مطلوب.")]
        //[Display(Name = "البريد الإلكتروني")]
        //[EmailAddress(ErrorMessage = "{0} غير صالح.")]
        //public string Email { get; set; }
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [EmailAddress(ErrorMessage = "{0} غير صالح.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(100, ErrorMessage = "{0} يجب أن تكون على الأقل {2} رمز.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقتان")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "صلاحية الحساب")]
        public string UserType { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني حقل مطلوب", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "{0} غير صالح.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "اسم المستخدم حقل مطلوب")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "الصلاحية حقل مطلوب")]
        [Display(Name = "صلاحية الحساب")]
        public string SelectedRole { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [EmailAddress(ErrorMessage = "{0} غير صالح.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [StringLength(100, ErrorMessage = "{0} يجب أن تكون على الأقل {2} رمز.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقتان")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "{0} حقل مطلوب.")]
        [EmailAddress(ErrorMessage = "{0} غير صالح.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }
    }
}
