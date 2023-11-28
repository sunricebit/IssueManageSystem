using System;
namespace IMS.ViewModels.Auth
{
	public class ForgotPasswordViewModel
	{
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

