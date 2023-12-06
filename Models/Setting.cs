using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Setting
    {
        public Setting()
        {
            Contacts = new HashSet<Contact>();
            PermissionPages = new HashSet<Permission>();
            PermissionRoles = new HashSet<Permission>();
            Posts = new HashSet<Post>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Permission> PermissionPages { get; set; }
        public virtual ICollection<Permission> PermissionRoles { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
