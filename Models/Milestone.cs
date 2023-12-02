using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Milestone
    {
        public Milestone()
        {
            Issues = new HashSet<Issue>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ClassId { get; set; }
        public int? AssignmentId { get; set; }

        public virtual Assignment? Assignment { get; set; }
        public virtual Class? Class { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
