namespace IMS.ViewModels.Milestone
{
    public class MilestoneViewModel
    {
           public MilestoneViewModel()
            {
                Issues = new HashSet<Issue>();
            }

            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string? Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? ProjectId { get; set; }
            public int ClassId { get; set; }
            public int? AssignmentId { get; set; }

            public virtual Assignment? Assignment { get; set; }
            public virtual IMS.Models.Class Class { get; set; } = null!;
            public virtual Project? Project { get; set; }
            public virtual ICollection<Issue> Issues { get; set; }
        }
    }
