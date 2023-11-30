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
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public bool? Gender { get; set; }
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
