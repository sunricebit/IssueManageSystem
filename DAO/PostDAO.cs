namespace IMS.DAO
{
    public partial class PostDAO
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string AuthorName { get; set; }
        public int? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
