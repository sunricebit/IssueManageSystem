using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Excerpt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublic { get; set; }
        public string? ImageUrl { get; set; }
        public int AuthorId { get; set; }
        public int? CategoryId { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual Setting? Category { get; set; }
    }
}
