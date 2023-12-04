using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int? ContactId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Contact? Contact { get; set; }
    }
}
