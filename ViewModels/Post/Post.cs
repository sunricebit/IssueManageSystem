﻿using System;
using System.Collections.Generic;

namespace IMS.ViewModels.Post
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsPublic { get; set; }
        public string? ImageUrl { get; set; }
        public string? Author { get; set; }
        public string Category { get; set; }

    }
}
