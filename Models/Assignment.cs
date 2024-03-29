﻿using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Assignment
    {
        public Assignment()
        {
            Milestones = new HashSet<Milestone>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int SubjectId { get; set; }
        public int Weight { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<Milestone> Milestones { get; set; }
    }
}
