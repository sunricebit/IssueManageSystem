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
using Firebase.Auth;
using Firebase.Storage;
using IMS.ViewModels.Post;
using System.Composition;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace IMS.Controllers
{
    public class PostsController : Controller
    {
        private readonly IMSContext _context;
        private static string ApiKey = "AIzaSyBjstBnMJX7h_NlJ5-vqcQE0V-Ldaztnk8";
        private static string Bucket = "imsmanagement-35781.appspot.com";
        private static string AuthEmail = "abc@gmail.com";
        private static string AuthPassword = "123456";
        public static string checkCreate;
        public PostsController(IMSContext context)
        {
            _context = context;
        }

        // GET:
        // Posts
        [Route("[controller]")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, string searchTerm = "", string filterCat="")
        {
            int? id = HttpContext.Session.GetUser()?.Id;

            var user = _context.Users.Include(u => u.Role).Where(u => u.Role.Type == "ROLE").SingleOrDefault(u => u.Id == id);
            ViewBag.CheckAccount = user.Role.Value;
            if (checkCreate == "Create successful!")
            {
                ViewBag.Success = "Create successful!";
                checkCreate = "";
            }
            if (id==null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Search = searchTerm;
                var cate = _context.Settings.Where(c => c.Type
                == "POST_CATEGORY").ToList();
                ViewBag.Setting = new SelectList(cate, "Value", "Value");

                
                if (user.Role.Value == "Admin" || user.Role.Value == "Marketer Manager")
                {
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
                            Where(p => p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).ToList();
                            
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


                }
                else
                {
                    if (searchTerm == null)
                    {
                        if (filterCat == "Category")
                        {
                            var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).Where(p=>p.Author.Id==id).ToList();
                            Pagination(page, pageSize, postList, searchTerm);
                        }
                        else
                        {
                            var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).
                            Where(p => p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).Where(p => p.Author.Id == id).ToList();
                            Pagination(page, pageSize, postList, searchTerm);
                        }

                    }
                    else
                    {
                        if (filterCat == "Category")
                        {
                            var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).
                           Where(p => p.Title.ToUpper().Contains(searchTerm.ToUpper())
                       || p.Author.Name.ToUpper().Contains(searchTerm.ToUpper())).Where(p => p.Author.Id == id).ToList();
                            Pagination(page, pageSize, postList, searchTerm);

                        }
                        else
                        {
                            var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).
                             Where(p => (p.Title.ToUpper().Contains(searchTerm.ToUpper())
                         || p.Author.Name.ToUpper().Contains(searchTerm.ToUpper()))
                         && p.Category.Value.ToUpper().Contains(filterCat.ToUpper())).Where(p => p.Author.Id == id).ToList();
                            Pagination(page, pageSize, postList, searchTerm);

                        }
                    }


                }
            }
           

            return View();
        }
        [Route("LoadPopupReport")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadPopupReport(int postIdCheckReport)
        {
            List<ReportData> reportDatas = new List<ReportData>();
            var reports = _context.Reports
                        .Where(r => r.PostId == postIdCheckReport).Include(r=>r.Reporter)
                        .ToList();
            
            foreach (var item in reports)
            {
                reportDatas.Add(new ReportData
                {
                    Content= item.Content,
                    Reporter=item.Reporter.Name,
                    CreatedAt=item.CreatedAt.ToString("dd/MM/yyyy"),
                });
            }
            var jsonOptions = new JsonSerializerOptions
            {
                Converters = { new NullHandlingConverter() },
                ReferenceHandler = ReferenceHandler.Preserve,
                IgnoreReadOnlyProperties = true, // Add this line
                                                 // Other options as needed
            };

            var jsonString = JsonSerializer.Serialize(reportDatas, jsonOptions);

            return Content(jsonString, "application/json");
        }
            public void Pagination(int page, int pageSize, List<Models.Post> postList, string searchTerm)
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
       
        // GET: Posts/Create
        public IActionResult Create()
        {
            var cate = _context.Settings.Where(c => c.Type == "POST_CATEGORY").ToList();
            ViewBag.Setting = new SelectList(cate, "Value", "Value");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Post post, IFormFile? imgFile, string category)
        {
            int? id = HttpContext.Session.GetUser()?.Id;
           
            if (ModelState.IsValid)
            {
                var categoryData = _context.Settings.SingleOrDefault(s => s.Value == category);
                if (imgFile != null && imgFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {

                        imgFile.CopyTo(fileStream);

                    }
                    var fileStream2 = new FileStream(filePath, FileMode.Open);
                    var downloadLink = await Upload(fileStream2, imgFile.FileName);

                    post.ImageUrl = downloadLink;
                }
                if (id!=null)
                {
                    var postViewModel = new IMS.Models.Post
                    {
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = post.ImageUrl,
                        AuthorId = (int)id,
                        CategoryId = categoryData.Id,
                        IsPublic = post.IsPublic,
                    };
                    _context.Add(postViewModel);
                    await _context.SaveChangesAsync();
                    checkCreate = "Create successful!";
                    return RedirectToAction(nameof(Index));
                }
               
            }
                return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            int? idAccount = HttpContext.Session.GetUser()?.Id;
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            var user = _context.Users.Include(u => u.Role).Where(u => u.Role.Type == "ROLE").SingleOrDefault(u => u.Id == idAccount);
            ViewBag.CheckAccount = user.Role.Value;

            var post = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p=>p.Id==id);
            var cate = _context.Settings.Where(c => c.Type == "POST_CATEGORY").Where(c=>c.Value!=post.Category.Value).ToList();
            var report = _context.Reports.Where(r => r.PostId == post.Id).Include(r=>r.Reporter).ToList();
            ViewBag.Setting = new SelectList(cate, "Value", "Value");
            var postViewModel = new IMS.ViewModels.Post.Post
            {
                Title = post.Title,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
                ImageUrl = post.ImageUrl,
                Author = post.Author.Name,
                Category = post.Category.Value,
                IsPublic= post.IsPublic,
                Reports= report,
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
        public async Task<IActionResult> Edit(int id, IMS.ViewModels.Post.Post post,string category, IFormFile? imgFile)
        {
            if (id != post.Id)
            {
                return NotFound();
            }
            if (imgFile != null && imgFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imgFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {

                    imgFile.CopyTo(fileStream);

                }
                var fileStream2 = new FileStream(filePath, FileMode.Open);
                var downloadLink = await Upload(fileStream2, imgFile.FileName);

                post.ImageUrl = downloadLink;
            }
            var cate = _context.Settings.SingleOrDefault(c => c.Value == category);
            if (ModelState.IsValid)
            {
                try
                {
                    IMS.Models.Post postUpdate = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p => p.Id == id);
                    if (imgFile != null)
                    {
                        postUpdate.Title = post.Title;
                        postUpdate.Description = post.Description;
                        postUpdate.CreatedAt = (DateTime)post.CreatedAt;
                        postUpdate.ImageUrl = post.ImageUrl;
                        postUpdate.Author.Name = post.Author;
                        postUpdate.CategoryId = cate.Id;
                        postUpdate.IsPublic = post.IsPublic;
                        _context.SaveChanges();
                    }
                    else
                    {
                        postUpdate.Title = post.Title;
                        postUpdate.Description = post.Description;
                        postUpdate.CreatedAt = (DateTime)post.CreatedAt;
                        postUpdate.Author.Name = post.Author;
                        postUpdate.CategoryId = cate.Id;
                        postUpdate.IsPublic = post.IsPublic;
                        _context.SaveChanges();
                    }
                    
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

       
        public async Task<string> Upload(FileStream stream, string filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                ).Child("User")
                 .Child(filename)
                 .PutAsync(stream, cancellation.Token);
            try
            {
                string link = await task;
                return link;

            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception was thrown : {0}", ex);
                return null;
            }
        }

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
                    //throw;
                    Console.Error.WriteLine("error");
                    }
                }
            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        public IActionResult DeleteAvatar(int id)
        {
            try
            {
                //int? id = HttpContext.Session.GetUser()?.Id;
                // Xử lý xóa avatar và cập nhật cơ sở dữ liệu
                var imgToDelete = _context.Posts.FirstOrDefault(a => a.Id == id);

                if (imgToDelete.ImageUrl != null)
                {
                    // Xóa file avatar
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Avatars", avatarToDelete.Avatar);
                    //System.IO.File.Delete(filePath);

                    // Xóa avatar trong cơ sở dữ liệu
                    // Cập nhật thông tin avatar trong cơ sở dữ liệu
                    imgToDelete.ImageUrl = null;
                    _context.SaveChanges();
                }
                ViewBag.Success = "Delete avatar successful";
                return RedirectToAction(nameof(Edit), new { id = id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error:" + ex.Message;
                return RedirectToAction(nameof(Index), new { tab = "userdetails" });
            }

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

        private bool PostExists(int id)
        {
          return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    public class NullHandlingConverter : JsonConverter<object>
    {
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(object), options);
        }
    }
}
