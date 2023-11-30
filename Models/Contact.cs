using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Name { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool? IsValid { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ContactTypeId { get; set; }

        public virtual Setting? ContactType { get; set; }
    }
}
