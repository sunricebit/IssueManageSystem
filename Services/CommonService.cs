using IMS.ViewModels.Common;

namespace IMS.Services
{
    public class CommonService:ICommonService
    {
        private readonly IMSContext _context;

        public CommonService(IMSContext context)
        {
            _context = context;
        }
        public IEnumerable<CategoryPostCount> GetSystemPublishedPostsByCategories()
        {
            var categoryPostCounts = _context.Posts
                .Where(p => p.IsPublic && p.CategoryId != null)
                .GroupBy(p => p.Category.Value)
                .Select(g => new CategoryPostCount
                {
                    Category = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(c => c.Count)
                .ToList();

            return categoryPostCounts;
        }
        public IEnumerable<AuthorPostCount> GetSystemPublishedPostsByTopAuthors()
        {
            var authorPostCounts = _context.Posts
                .Where(p => p.IsPublic)
                .GroupBy(p => p.Author.Name)
                .Select(g => new AuthorPostCount
                {
                    Author = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(a => a.Count)
                .Take(5)
                .ToList();

            return authorPostCounts;
        }
    }
}
