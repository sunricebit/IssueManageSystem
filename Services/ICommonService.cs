using IMS.ViewModels.Common;

namespace IMS.Services
{
    public interface ICommonService
    {
        public IEnumerable<AuthorPostCount> GetSystemPublishedPostsByTopAuthors();
        public IEnumerable<CategoryPostCount> GetSystemPublishedPostsByCategories();
    }
}
