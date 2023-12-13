using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Project
    {
        public Project()
        {
            IssueSettings = new HashSet<IssueSetting>();
            Issues = new HashSet<Issue>();
            Milestones = new HashSet<Milestone>();
            Students = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public bool? Status { get; set; }
        public string? Description { get; set; }
        public int ClassId { get; set; }
        public int? LeaderId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual User? Leader { get; set; }
        public virtual ICollection<IssueSetting> IssueSettings { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<Milestone> Milestones { get; set; }

        public virtual ICollection<User> Students { get; set; }
    }
}
