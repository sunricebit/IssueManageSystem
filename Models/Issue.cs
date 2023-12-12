using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Issue
    {
        public Issue()
        {
            InverseParentIssue = new HashSet<Issue>();
            IssueSettings = new HashSet<IssueSetting>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? MilestoneId { get; set; }
        public int? ProjectId { get; set; }
        public int AuthorId { get; set; }
        public int? AssigneeId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int ProcessId { get; set; }
        public int? ParentIssueId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual User? Assignee { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual Milestone? Milestone { get; set; }
        public virtual Issue? ParentIssue { get; set; }
        public virtual Setting Process { get; set; } = null!;
        public virtual Project? Project { get; set; }
        public virtual Setting Status { get; set; } = null!;
        public virtual Setting Type { get; set; } = null!;
        public virtual ICollection<Issue> InverseParentIssue { get; set; }
        public virtual ICollection<IssueSetting> IssueSettings { get; set; }
    }
}
