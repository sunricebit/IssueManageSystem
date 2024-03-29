﻿using IMS.Models;
using IMS.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IMS.Controllers
{
    public class BlogController : Controller
    {
        private readonly IMSContext _context;
        public static string checkSendReport;
        public BlogController(IMSContext context)
        {
            _context = context;
        }

        [Route("[controller]")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 8, string searchTerm = "", string filterCat = "", string filterAuthor = "")
        {
            int? id = HttpContext.Session.GetUser()?.Id;
            ViewBag.CheckAccount = id;
            ViewBag.Search = searchTerm;
            ViewBag.filterCat = filterCat;
            ViewBag.filterAuthor = filterAuthor;
            var cate = _context.Settings.Where(c => c.Type
            == "POST_CATEGORY").ToList();
            ViewBag.Setting = new SelectList(cate, "Value", "Value");
            var author = _context.Posts.Where(p => p.IsPublic == true).Include(p => p.Author).GroupBy(p => p.Author.Name)
            .Select(group => group.First()).Distinct().ToList();
            List<PostDAO> authorDuplicate = new List<PostDAO>();
           
            ViewBag.Author = new SelectList(author, "Author.Name", "Author.Name");
            if (searchTerm == null || searchTerm == "")
            {
                List<PostDAO> postList = new List<PostDAO>();
                if (filterAuthor== "Author" && filterCat== "Category")
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p=>p.Category).Where(p => p.IsPublic == true).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id= post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName=post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
               else if (filterAuthor == "Author" && filterCat != "Category")
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p=>p.Category).Where(p => p.IsPublic == true).Where(p=>p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
                else if(filterAuthor != "Author" && filterCat == "Category")
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p => p.IsPublic == true).Where(p => p.Author.Name.ToUpper().Contains(filterAuthor.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
                else
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p => p.IsPublic == true).Where(p => p.Author.Name.ToUpper().Contains(filterAuthor.ToUpper())&& p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
            }
            else
            {
                List<PostDAO> postList = new List<PostDAO>();
                if (filterAuthor == "Author" && filterCat == "Category")
                {
                    var Post = _context.Posts.Include(p => p.Author).Where(p => p.IsPublic == true).Where(p => p.IsPublic == true).Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper()) || p.Excerpt.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
                else if (filterAuthor == "Author" && filterCat != "Category")
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p => p.IsPublic == true).Where(p => p.Category.Value.ToUpper().Contains(filterCat.ToUpper()) ).Where(p => p.IsPublic == true).Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper()) || p.Excerpt.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
                else if (filterAuthor != "Author" && filterCat == "Category")
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p => p.IsPublic == true).Where(p => p.Author.Name.ToUpper().Contains(filterAuthor.ToUpper())).Where(p => p.IsPublic == true).Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper())  || p.Excerpt.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }
                else
                {
                    var Post = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p => p.IsPublic == true).Where(p => p.Author.Name.ToUpper().Contains(filterAuthor.ToUpper()) && p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).Where(p => p.IsPublic == true).Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper())  || p.Excerpt.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                    foreach (var post in Post)
                    {
                        postList.Add(new PostDAO
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Description = post.Description,
                            ImageUrl = post.ImageUrl,
                            Excerpt = post.Excerpt,
                            AuthorName = post.Author.Name,
                            CategoryName = post.Category.Value,
                            CreatedAt = post.CreatedAt,
                        });
                    }
                    Pagination(page, pageSize, postList, searchTerm);
                }

            }

            return View();

        }

        public void Pagination(int page, int pageSize, List<PostDAO> postList, string searchTerm)
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

        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            if(checkSendReport== "Send report successful")
            {
                ViewBag.Report = "Send report successful";
                checkSendReport = "";
            }
            var post = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            int? idAccount = HttpContext.Session.GetUser()?.Id;
            ViewBag.CheckAccount = idAccount;
            ViewBag.CheckPost = id;
            var cate = _context.Settings.Where(c => c.Type
           == "POST_CATEGORY").ToList();
            ViewBag.Setting = new SelectList(cate, "Value", "Value");
            var author = _context.Posts.Where(p => p.IsPublic == true).Include(p => p.Author).GroupBy(p => p.Author.Name)
            .Select(group => group.First()).Distinct().ToList();
            List<PostDAO> authorDuplicate = new List<PostDAO>();

            ViewBag.Author = new SelectList(author, "Author.Name", "Author.Name");
            var randomPostsLienQuan = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p=>p.Category.Value==post.Category.Value)
            .OrderBy(p => Guid.NewGuid()) // Sắp xếp ngẫu nhiên bằng cách sử dụng Guid.NewGuid()
            .Take(3)
            .ToList();
            var randomPosts = _context.Posts.Include(p => p.Author).Include(p => p.Category).OrderBy(p => Guid.NewGuid()) // Sắp xếp ngẫu nhiên bằng cách sử dụng Guid.NewGuid()
            .Take(5)
            .ToList();
            ViewBag.RandomLienQuan = randomPostsLienQuan;
            ViewBag.Random = randomPosts;
            var postViewModel = new IMS.DAO.PostDAO
                {
                    Title = post.Title,
                    Description= post.Description,
                    CreatedAt = post.CreatedAt,
                    Excerpt =post.Excerpt,
                    ImageUrl = post.ImageUrl,
                    UpdatedAt=post.UpdatedAt,
                    AuthorName = post.Author.Name,
                    CategoryName = post.Category.Value,

                };
            return View(postViewModel);


        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReport(int idPost, string content)
        {
            int? id = HttpContext.Session.GetUser()?.Id;
            if (id == null)
            {
                return NotFound();
            }
            else {
                var report = new IMS.Models.Report
                {
                    Content = content,
                    PostId = idPost,
                    ReporterId = (int)id,
                    CreatedAt = DateTime.Now,
                };
                _context.Add(report);
                await _context.SaveChangesAsync();
                checkSendReport = "Send report successful";
                return RedirectToAction("Detail", "Blog", new { id = idPost });
            }
            
        }

    }
}
