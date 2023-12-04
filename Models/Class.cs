using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Class
    {
        public Class()
        {
            IssueSettings = new HashSet<IssueSetting>();
            Milestones = new HashSet<Milestone>();
            Projects = new HashSet<Project>();
            Students = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? TeacherId { get; set; }
        public int? SubjectId { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual User? Teacher { get; set; }
        public virtual ICollection<IssueSetting> IssueSettings { get; set; }
        public virtual ICollection<Milestone> Milestones { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<User> Students { get; set; }
    }
}
