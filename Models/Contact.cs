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
    }
}
