using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Firebase.Auth;
using Firebase.Storage;
using IMS.ViewModels.Post;
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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6, string searchTerm = "", string filterCat="", string filterAuthor = "", string filterPublic = "All")
        {
            int? id = HttpContext.Session.GetUser()?.Id;

            var user = _context.Users.Include(u => u.Role).Where(u => u.Role.Type == "ROLE").SingleOrDefault(u => u.Id == id);
            ViewBag.CheckAccount = user.Role.Value;
            if (checkCreate == "Create successful!")
            {
                ViewBag.Success = "Create successful!";
                checkCreate = "";
            }
            if (id == null)
            {https://localhost:7104/Posts
                return NotFound();
            }
            else
            {
                ViewBag.Search = searchTerm;
                ViewBag.filterCat = filterCat;
                ViewBag.filterAuthor = filterAuthor;
                ViewBag.filterPublic = filterPublic;
                var cate = _context.Settings.Where(c => c.Type
                == "POST_CATEGORY").ToList();
                ViewBag.Setting = new SelectList(cate, "Value", "Value");
                var author = _context.Posts.Include(p => p.Author).GroupBy(p => p.Author.Name)
            .Select(group => group.First()).Distinct().ToList();
                ViewBag.Author = new SelectList(author, "Author.Name", "Author.Name");
                var postList = _context.Posts.Include(p => p.Author).Include(p => p.Category).ToList();
                Pagination(page, pageSize, postList, searchTerm);
                if (user.Role.Value == "Admin" || user.Role.Value == "Marketer Manager")
                {
                    if (searchTerm == null|| searchTerm == "")
                    {
                        if (filterCat != "Category" && !string.IsNullOrEmpty(filterCat))
                        {
                            postList = postList.Where(p => p.Category.Value==filterCat).ToList();
                        }

                        if (filterAuthor != "Author" && !string.IsNullOrEmpty(filterAuthor))
                        {
                            postList = postList.Where(p => p.Author.Name==filterAuthor).ToList();
                        }

                        if (filterPublic != "All")
                        {
                            bool isPublic = filterPublic == "Yes";
                            postList = postList.Where(p => p.IsPublic == isPublic).ToList();
                        }

                        Pagination(page, pageSize, postList, searchTerm);
                    }
                    else
                    {
                        postList = postList.Where(p => p.Excerpt.ToUpper().Contains(searchTerm.ToUpper()) || p.Title.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                        if (filterCat != "Category" && !string.IsNullOrEmpty(filterCat))
                        {
                            postList = postList.Where(p => p.Category.Value == filterCat).ToList();
                        }

                        if (filterAuthor != "Author" && !string.IsNullOrEmpty(filterAuthor))
                        {
                            postList = postList.Where(p => p.Author.Name == filterAuthor).ToList();
                        }

                        if (filterPublic != "All")
                        {
                            bool isPublic = filterPublic == "Yes";
                            postList = postList.Where(p => p.IsPublic == isPublic).ToList();
                        }

                        Pagination(page, pageSize, postList, searchTerm);
                    }


                }
                else
                {
                     postList = postList.Where(p => p.AuthorId == id).ToList();
                    if (searchTerm == null||searchTerm == "")
                    {
                        if (filterCat != "Category" && !string.IsNullOrEmpty(filterCat))
                        {
                            postList = postList.Where(p => p.Category.Value == filterCat).ToList();
                        }

                        if (filterAuthor != "Author" && !string.IsNullOrEmpty(filterAuthor))
                        {
                            postList = postList.Where(p => p.Author.Name == filterAuthor).ToList();
                        }

                        if (filterPublic != "All")
                        {
                            bool isPublic = filterPublic == "Yes";
                            postList = postList.Where(p => p.IsPublic == isPublic).ToList();
                        }

                        Pagination(page, pageSize, postList, searchTerm);
                    }
                    else
                    {
                        postList = postList.Where(p => p.Excerpt.ToUpper().Contains(searchTerm.ToUpper()) || p.Title.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                        if (filterCat != "Category" && !string.IsNullOrEmpty(filterCat))
                        {
                            postList = postList.Where(p => p.Category.Value == filterCat).ToList();
                        }

                        if (filterAuthor != "Author" && !string.IsNullOrEmpty(filterAuthor))
                        {
                            postList = postList.Where(p => p.Author.Name == filterAuthor).ToList();
                        }

                        if (filterPublic != "All")
                        {
                            bool isPublic = filterPublic == "Yes";
                            postList = postList.Where(p => p.IsPublic == isPublic).ToList();
                        }

                        Pagination(page, pageSize, postList, searchTerm);
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
            return  View(); 
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Post post, IFormFile? imgFile, string category)
        {
            int? id = HttpContext.Session.GetUser()?.Id;
           
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
                if (id != null)
                {
                    var postViewModel = new IMS.Models.Post
                    {
                        Title = post.Title,
                        Description = post.Description,
                        ImageUrl = post.ImageUrl,
                        //AuthorId = 1,
                        AuthorId = (int)id,
                        Excerpt =post.Excerpt,
                        UpdatedAt=DateTime.Now,
                        CategoryId = categoryData.Id,
                        IsPublic = post.IsPublic,
                    };
                    _context.Add(postViewModel);
                    await _context.SaveChangesAsync();
                    checkCreate = "Create successful!";
                    return RedirectToAction(nameof(Index));
                }

            return View(post);
        }

        public async Task DeleteFromFirebase(string filename)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                var cancellation = new CancellationTokenSource();
                var storage = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                );
                var oldAvatarPath = $"User/{filename}";
                await storage.Child(oldAvatarPath).DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred during deletion: {0}", ex);
            }
        }

        public async Task<IActionResult> Detail(int? id)
        {
            int? idAccount = HttpContext.Session.GetUser()?.Id;
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            var user = _context.Users.Include(u => u.Role).Where(u => u.Role.Type == "ROLE").SingleOrDefault(u => u.Id == idAccount);
            ViewBag.CheckAccount = user.Role.Value;

            var post = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p => p.Id == id);
            var report = _context.Reports.Where(r => r.PostId == post.Id).Include(r => r.Reporter).ToList();
            var postViewModel = new PostDAO
            {
                Id= id,
                Title = post.Title,
                Description = post.Description,
                Excerpt=post.Excerpt,
                UpdatedAt = post.UpdatedAt,
                ImageUrl = post.ImageUrl,
                AuthorName = post.Author.Name,
                CategoryName = post.Category.Value,
                Reports = report,
            };
            if (post == null)
            {
                return NotFound();
            }
            return View(postViewModel);
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
            
            var postViewModel = new PostDAO
            {
                Id= id,
                Title = post.Title,
                Description = post.Description,
                CreatedAt = post.CreatedAt,
                Excerpt =post.Excerpt,
                ImageUrl = post.ImageUrl,
                AuthorName = post.Author.Name,
                CategoryName = post.Category.Value,
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
        public async Task<IActionResult> Edit(int id, PostDAO post,string category, IFormFile? imgFile)
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
                var postOld = _context.Posts.SingleOrDefault(p=>p.Id== id);
                if (postOld.ImageUrl != null && postOld.ImageUrl != "https://firebasestorage.googleapis.com/v0/b/imsmanagement-35781.appspot.com/o/User%2Fdefault_avatar.jpg?alt=media&token=c9ec5062-d46b-4009-a04a-4fbeb5532005")
                {
                    await DeleteFromFirebase(postOld.ImageUrl);
                }
                var downloadLink = await Upload(fileStream2, imgFile.FileName);

                post.ImageUrl = downloadLink;
            }
            var cate = _context.Settings.SingleOrDefault(c => c.Value == category);
            
                try
                {
                    IMS.Models.Post postUpdate = _context.Posts.Include(p => p.Author).Include(p => p.Category).SingleOrDefault(p => p.Id == id);
                    if (imgFile != null)
                    {
                        postUpdate.Title = post.Title;
                        postUpdate.Description = post.Description;
                        postUpdate.ImageUrl = post.ImageUrl;
                        postUpdate.UpdatedAt = DateTime.Now;
                        postUpdate.Excerpt = post.Excerpt;
                        postUpdate.Author.Name = post.AuthorName;
                        postUpdate.CategoryId = cate.Id;
                        postUpdate.IsPublic = post.IsPublic;
                        _context.SaveChanges();
                    }
                    else
                    {
                        postUpdate.Title = post.Title;
                        postUpdate.Description = post.Description;
                        postUpdate.Excerpt = post.Excerpt;
                        postUpdate.UpdatedAt = DateTime.Now;
                        postUpdate.Author.Name = post.AuthorName;
                        postUpdate.CategoryId = cate.Id;
                        postUpdate.IsPublic = post.IsPublic;
                        _context.SaveChanges();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                        return NotFound();
                }
                return RedirectToAction("Detail", "Posts", new { id = id });

           
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
        public async Task<IActionResult> DeleteImageAsync(int id)
        {
            try
            {
                //int? id = HttpContext.Session.GetUser()?.Id;
                // Xử lý xóa avatar và cập nhật cơ sở dữ liệu
                var imgToDelete = _context.Posts.FirstOrDefault(a => a.Id == id);

                if (imgToDelete.ImageUrl != null)
                {
                    await DeleteFromFirebase(imgToDelete.ImageUrl);
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
