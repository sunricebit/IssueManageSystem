using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter an Email.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Phone Number. Please enter a valid numeric phone number.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please enter a Name.")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a Message.")]
        [StringLength(500, ErrorMessage = "Message must not exceed 500 characters.")]
        public string Message { get; set; } = null!;
        public bool? IsValid { get; set; }
        [Required(ErrorMessage = "Please enter a Reason.")]
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ContactTypeId { get; set; }

        public virtual Setting? ContactType { get; set; }
    }
}
