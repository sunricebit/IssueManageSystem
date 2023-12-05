namespace IMS.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string? Avatar { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public string? ConfirmToken { get; set; }
        public string? ResetToken { get; set; }
    }
}

