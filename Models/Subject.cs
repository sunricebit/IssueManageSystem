using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Assignments = new HashSet<Assignment>();
            Classes = new HashSet<Class>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int SubjectManagerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User SubjectManager { get; set; } = null!;
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
