using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class User
    {
        public User()
        {
            Classes = new HashSet<Class>();
            Contacts = new HashSet<Contact>();
            IssueAssignees = new HashSet<Issue>();
            IssueAuthors = new HashSet<Issue>();
            Posts = new HashSet<Post>();
            Projects = new HashSet<Project>();
            ClassesNavigation = new HashSet<Class>();
            ProjectsNavigation = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? Status { get; set; }
        public DateTime? LstAccessTime { get; set; }
        public string? ConfirmToken { get; set; }
        public string? ResetToken { get; set; }

        public virtual Setting Role { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Issue> IssueAssignees { get; set; }
        public virtual ICollection<Issue> IssueAuthors { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<Class> ClassesNavigation { get; set; }
        public virtual ICollection<Project> ProjectsNavigation { get; set; }
    }
}
