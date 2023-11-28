﻿using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UserId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual User User { get; set; } = null!;
    }
}