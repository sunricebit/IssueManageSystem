using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Report
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public int ReporterId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User Reporter { get; set; } = null!;
    }
}
