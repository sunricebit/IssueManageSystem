using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Permission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Page { get; set; } = null!;
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        public virtual Setting Role { get; set; } = null!;
    }
}
