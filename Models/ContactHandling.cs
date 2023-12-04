 using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class ContactHandling
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Note { get; set; }
        public int? ContactId { get; set; }

        public virtual Contact? Contact { get; set; }
    }
}
