using IMS.ViewModels.Common;

namespace IMS.Services
{
    public interface ICommonService
    {
        public IEnumerable<AuthorPostCount> GetSystemPublishedPostsByTopAuthors();
        public IEnumerable<CategoryPostCount> GetSystemPublishedPostsByCategories();
        public IEnumerable<Setting> GetCategory();
        public List<int> userPostByCategory(int categoryId, int authorId);
    }
}
