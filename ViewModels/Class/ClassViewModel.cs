
namespace IMS.ViewModels.Class
{
    public  class ClassViewModel
    {
        public ClassViewModel()
        {
            IssueSettings = new HashSet<IssueSetting>();
            Milestones = new HashSet<IMS.Models.Milestone>();
            Projects = new HashSet<Project>();
            Students = new HashSet<Models.User>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? TeacherId { get; set; }
        public int? SubjectId { get; set; }
        public bool IsActive { get; set; }

        public virtual Models.Subject? Subject { get; set; }
        public virtual Models.User? Teacher { get; set; }
        public virtual ICollection<IssueSetting> IssueSettings { get; set; }
        public virtual ICollection<IMS.Models.Milestone> Milestones { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Models.User> Students { get; set; }
    }
}
