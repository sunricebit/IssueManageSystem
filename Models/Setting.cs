using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Setting
    {
        public Setting()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Value { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
