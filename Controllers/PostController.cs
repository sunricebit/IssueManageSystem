using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    public class PostController : Controller
    {
        private readonly IMSContext _context;
        public PostController(IMSContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1,int pageSize= 8,string searchTerm="")
        {
            ViewBag.Search = searchTerm;
            if (searchTerm == null)
            {
                List<PostDAO> postList = new List<PostDAO>();
                var Post = _context.Posts.Include(p => p.Author).ToList();
                foreach (var post in Post)
                {
                    postList.Add(new PostDAO
                    {
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = post.ImageUrl,
                        AuthorName = post.Author.Name,
                        CreatedAt = post.CreatedAt,
                    });
                }
                Pagination(page, pageSize , postList, searchTerm);
            }
            else
            {
                List<PostDAO> postList = new List<PostDAO>();
                var Post = _context.Posts.Include(p=>p.Author).Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper())||p.Author.Name.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                foreach (var post in Post)
                {
                    postList.Add(new PostDAO
                    {
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = post.ImageUrl,
                        AuthorName = post.Author.Name,
                        CreatedAt = post.CreatedAt,
                    });
                }
                Pagination(page, pageSize , postList, searchTerm);
            }
            
            return View();
        }
        public void Pagination(int page, int pageSize, List<PostDAO> postList ,string searchTerm)
        {
            
            // Tính toán số trang và dữ liệu cho trang hiện tại
            var totalItems = postList.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var itemsOnPage = postList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu và thông tin phân trang cho view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = searchTerm;
            ViewBag.PostList = itemsOnPage;
        }
    }
}
