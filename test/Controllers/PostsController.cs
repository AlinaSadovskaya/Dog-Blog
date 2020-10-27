using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using test.Services.BusinessLogic;
using test.Domain.Core;
using test.Domain.Interfaces;
using test.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using test.ViewModels;

namespace test.Controllers
{
    public class PostsController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly BlogContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<PostsController> _logger;
        private readonly ImageService _imageService;
        private readonly PostRepository _postRepository;
        List<Topic> _topics;
        public PostsController(UserManager<User> userManager, BlogContext context, IWebHostEnvironment appEnvironment, ImageService imageService, PostRepository postRepository, ILogger<PostsController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
            _imageService = imageService;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index(int? TopicId)
        {
            this._topics = await _context.Topics.ToListAsync();
            PostIndexViewModel ivm;
            
            if (TopicId != null)
            {
                List<Post> posts;
                try
                {
                    posts = await _context.Posts.Where(e => e.Topic.TopicId == TopicId).ToListAsync();
                }
                catch (Exception)
                {
                    _logger.LogError("Doesn't exist id. Controller:Posts. Action:Index");
                    return RedirectPermanent("~/Error/Index?statusCode=404");
                }
                ivm = new PostIndexViewModel { Posts = posts, Topics = _topics };
                return View(ivm);
            }
            ivm = new PostIndexViewModel { Posts = await _context.Posts.ToListAsync(), Topics = _topics };
            return View(ivm);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var post = await _postRepository.FirstOrDefaultAsync(id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist post. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var user = await _context.Users.FindAsync(post.UserId);
            if (user == null)
            {
                _logger.LogError("Doesn't exist user. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            ViewData["UserName"] = user.UserName;
            List<Comment> comments = new List<Comment>();
            foreach (var item in _context.Comments.Include(s => s.Post).Where(s => s.PostId == post.PostId).ToList())
            {
                item.User = await _context.Users.FindAsync(item.UserId);
                comments.Add(item);
            }

            ViewData["Comment"] = comments;

            return View(post);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!user.isBlocked)
            {
                SelectList topics = new SelectList(_context.Topics, "TopicID", "TopicName");
                ViewBag.Topics = topics;
                ViewData["topics"] = _context.Topics.ToList();
                return View();
            }
            else
            {
                ViewData["Text"] = "Ваша страница заблокирована администратором";
                return View("~/Views/Shared/TextPage.cshtml");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Text,TopicId")] Post post, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/Files/posts/" + uploadedFile.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    post.Image = path;
                    _context.SaveChanges();
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                post.User = user;
                post.DateTime = DateTime.Now;
                post.Topic = await _context.Topics.FindAsync(post.TopicId);
                await _postRepository.Create(post);
                return RedirectPermanent("~/Posts/Index");
            }
            return View(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Edit. id = null");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var post = await _postRepository.FirstOrDefaultAsync(id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist post. Controller:Post. Action:Edit. post = null");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            var user = await _userManager.FindByIdAsync(post.UserId);
            if (User.Identity.Name.ToString() != user.UserName || !User.IsInRole("admin"))
            {
                _logger.LogError("Doesn't exist user. Controller:Post. Action:Edit");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Text,Image")] Post post, IFormFile uploadedFile)
        {
            if (id != post.PostId)
            {
                _logger.LogError("Doesn't exist id. Controller:Article. Action:Edit");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            Post post1 = await _postRepository.FirstOrDefaultAsync(post.PostId);
            if (ModelState.IsValid)
            {
                try
                {
                    if (post.Text != post1.Text && post1 != null)
                        post1.Text = post.Text;
                    if (post.Title != post1.Title && post1 != null)
                        post1.Title = post.Title;
                    if (post.Image != post1.Image && post1.Image != null)
                    {
                        post1.Image = await _imageService.SaveImageAsync(uploadedFile, 1);
                    }
                    await _postRepository.Update(post1);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        _logger.LogError("Doesn't exist db. Controller:Article. Action:Edit");
                        return RedirectPermanent("~/Error/Index?statusCode=404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectPermanent("~/");
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError("Doesn't exist id. Controller:Post. Action:Delete");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            var post = await _postRepository.FirstOrDefaultAsync(id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist areticle. Controller:Post. Action:Delete");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            return View(post);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _postRepository.FirstOrDefaultAsync(id);
            await _postRepository.Remove(post);
            return RedirectPermanent("~/");
        }

        private bool PostExists(int id)
        {
            return _postRepository.Any(id);
        }
    }
}
