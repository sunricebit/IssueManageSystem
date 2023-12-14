using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class IssueSetting
    {
        public IssueSetting()
        {
            IssueProcesses = new HashSet<Issue>();
            IssueStatuses = new HashSet<Issue>();
            IssueTypes = new HashSet<Issue>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Description { get; set; }
        public bool? Status { get; set; }
        public int? ClassId { get; set; }
        public int? ProjectId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Project? Project { get; set; }
        public virtual ICollection<Issue> IssueProcesses { get; set; }
        public virtual ICollection<Issue> IssueStatuses { get; set; }
        public virtual ICollection<Issue> IssueTypes { get; set; }
    }
}
