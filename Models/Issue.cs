using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Issue
    {
        public Issue()
        {
            Issuesettings = new HashSet<Issuesetting>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? MilestoneId { get; set; }
        public int? ProjectId { get; set; }
        public int AuthorId { get; set; }
        public int? AssigneeId { get; set; }

        public virtual User? Assignee { get; set; }
        public virtual User Author { get; set; } = null!;
        public virtual Milestone? Milestone { get; set; }
        public virtual Project? Project { get; set; }
        public virtual ICollection<Issuesetting> Issuesettings { get; set; }
    }
}
