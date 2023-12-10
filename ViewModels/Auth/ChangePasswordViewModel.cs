using System;
namespace IMS.ViewModels.Auth
{
	public class ChangePasswordViewModel
	{
        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [StringLength(64, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&^_-]+$", ErrorMessage = "The password must contain at least one letter and one digit.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [StringLength(64, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&^_-]+$", ErrorMessage = "The password must contain at least one letter and one digit.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your confirm password.")]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword == OldPassword)
            {
                yield return new ValidationResult("New password must be different from the current password.", new[] { nameof(NewPassword) });
            }
        }
    }
}

