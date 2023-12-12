namespace IMS.DAO
{
    public partial class PostDAO
    {
        public int? Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Excerpt { get; set; }
        public string? AuthorName { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
