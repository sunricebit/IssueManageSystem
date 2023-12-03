using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Project
    {
        public Project()
        {
            Issues = new HashSet<Issue>();
            Issuesettings = new HashSet<Issuesetting>();
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
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<Issuesetting> Issuesettings { get; set; }

        public virtual ICollection<User> Students { get; set; }
    }
}
