using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IMS.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Bibliography;

namespace IMS.Controllers
{
    public class PostsController : Controller
    {
        private readonly IMSContext _context;

        public PostsController(IMSContext context)
        {
            _context = context;
        }

        // GET:
        // Posts
        [Route("[controller]")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, string searchTerm = "", string filterCat="")
        {
            ViewBag.Search = searchTerm;
            var cate = _context.Settings.Where(c=>c.Type
            == "POST_CATEGORY").ToList();
            ViewBag.Setting = new SelectList(cate, "Value", "Value");
            if (searchTerm == null)
            {
                if (filterCat == "Category")
                {
                    var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).ToList();
                    Pagination(page, pageSize, postList, searchTerm);
                }
                else
                {
                    var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).
                    Where(p =>  p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).ToList();
                    Pagination(page, pageSize, postList, searchTerm);
                }
               
            }
            else
            {
                if (filterCat == "Category")
                {
                    var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).
                   Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper())
               || p.Author.Name.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                    Pagination(page, pageSize, postList, searchTerm);

                }
                else
                {
                    var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).
                     Where(p => (p.Title.ToUpper().Contains(searchTerm.ToUpper())
                 || p.Author.Name.ToUpper().Contains(searchTerm.ToUpper()))
                 && p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).ToList();
                    Pagination(page, pageSize, postList, searchTerm);

                }
            }

          
            return View();
        }
       
        public void Pagination(int page, int pageSize, List<Post> postList, string searchTerm)
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
        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.Settings, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedAt,UpdatedAt,IsPublic,ImageUrl,AuthorId,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p=>p.Id==id);
            var postViewModel = new IMS.ViewModels.Post.Post
            {
                Title = post.Title,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
                ImageUrl = post.ImageUrl,
                Author = post.Author.Name,
                Category = post.Category.Value,
            };
            if (post == null)
            {
                return NotFound();
            }
            
            return View(postViewModel);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IMS.ViewModels.Post.Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IMS.Models.Post postUpdate = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p => p.Id == id); ;
                    postUpdate.Title = post.Title;
                    postUpdate.Description = post.Description;
                    postUpdate.CreatedAt = post.CreatedAt;
                    postUpdate.ImageUrl = post.ImageUrl;
                    postUpdate.Author.Name = post.Author;
                    postUpdate.Category.Value = post.Category;
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            return RedirectToAction(nameof(Edit));
        }
        //[Route("UpdateIsPublic")]
        [Route("UpdateIsPublic")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateIsPublic(int postId, bool isPublic)
        {
            if (postId == null )
            {
                return NotFound();
            }

            var post =  _context.Posts.SingleOrDefault(p=>p.Id == postId);
            if (post == null)
            {
                return NotFound();
            }
                try
                {
                Models.Post postUpdate = _context.Posts.Find(postId);
                postUpdate.IsPublic = isPublic;
                _context.SaveChanges();
                //TempData["SuccessMessage"] = "success";
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            return RedirectToAction(nameof(Index), new { success = "success" });

        }


        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'IMSContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
