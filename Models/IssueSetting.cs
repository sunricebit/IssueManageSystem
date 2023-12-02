using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class IssueSetting
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool? Status { get; set; }
        public int? ClassId { get; set; }
        public int? IssueId { get; set; }
        public int? ProjectId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Issue? Issue { get; set; }
        public virtual Project? Project { get; set; }
    }
}
