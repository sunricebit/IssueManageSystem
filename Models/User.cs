using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter an Email.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public bool? Gender { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Phone Number. Please enter a valid numeric phone number.")]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public DateTime? LstAccessTime { get; set; }
        public string? ConfirmToken { get; set; }
        public string? ResetToken { get; set; }

        public virtual Setting Role { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; }
    }
}
