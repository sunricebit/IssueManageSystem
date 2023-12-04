using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Contact
    {
        public Contact()
        {
            Messages = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public bool? IsValid { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CarerId { get; set; }
        public int ContactTypeId { get; set; }

        public virtual User? Carer { get; set; }
        public virtual Setting ContactType { get; set; } = null!;
        public virtual ICollection<Message> Messages { get; set; }
    }
}
