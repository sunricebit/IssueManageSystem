using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Setting
    {
        public Setting()
        {
            Contacts = new HashSet<Contact>();
            Posts = new HashSet<Post>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
