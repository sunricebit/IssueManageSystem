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
            Students = new HashSet<User>();
        }

        public int Id { get; set; }
        public string EnglishName { get; set; } = null!;
        public string VietnameseName { get; set; } = null!;
        public bool? Status { get; set; }
        public string Description { get; set; } = null!;
        public int? ClassId { get; set; }
        public int? LeaderId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual User? Leader { get; set; }
        public virtual ICollection<IssueSetting> IssueSettings { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }

        public virtual ICollection<User> Students { get; set; }
    }
}
