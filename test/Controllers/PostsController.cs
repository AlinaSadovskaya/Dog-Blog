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
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<PostsController> _logger;
        private readonly ImageService _imageService;
        private readonly PostRepository _postRepository;
        private readonly TopicRepository _topicRepository;
        private readonly CommentRepository _commentRepository;
        private readonly Repository<User, string> _userRepository;
        List<Topic> _topics;
        public PostsController(UserManager<User> userManager,IWebHostEnvironment appEnvironment, ImageService imageService, PostRepository postRepository, ILogger<PostsController> logger, TopicRepository topicRepository, CommentRepository commentRepository, Repository<User, string> userRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
            _imageService = imageService;
            _postRepository = postRepository;
            _topicRepository = topicRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(int? TopicId)
        {
            this._topics = await _topicRepository.FindAll();
            PostIndexViewModel ivm;
            
            if (TopicId != null)
            {
                int id = (int)TopicId;
                List<Post> posts;
                try
                {
                    posts = _postRepository.FindAllByTopic(id);
                }
                catch (Exception)
                {
                    _logger.LogError("Doesn't exist id. Controller:Posts. Action:Index");
                    return RedirectPermanent("~/Error/Index?statusCode=404");
                }
                ivm = new PostIndexViewModel { Posts = posts, Topics = _topics };
                return View(ivm);
            }
            ivm = new PostIndexViewModel { Posts = await _postRepository.FindAll(), Topics = _topics };
            return View(ivm);

        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postRepository.FirstOrDefaultAsync(id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist post. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }

            User user = await _userRepository.FindAsyncById(post.UserId);
            if (user == null)
            {
                _logger.LogError("Doesn't exist user. Controller:Post. Action:Details");
                return RedirectPermanent("~/Error/Index?statusCode=404");
            }
            ViewData["UserName"] = user.UserName;
            List<Comment> comments = new List<Comment>();
            foreach (var item in  _commentRepository.FindAllByPost(post.PostId).ToList())
            {
                item.User = await _userRepository.FindAsyncById(item.UserId);
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
                SelectList topics = new SelectList(_topicRepository.getSet(), "TopicID", "TopicName");
                ViewBag.Topics = topics;
                ViewData["topics"] = await _topicRepository.FindAll();
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
                if (uploadedFile != null && uploadedFile != null && uploadedFile.ContentType.ToLower().Contains("image"))
                {
                    post.Image = await _imageService.SaveImageAsync(uploadedFile, 1);
                }
                else
                {
                    ModelState.AddModelError("Image", "Некорректный формат");
                    ViewData["topics"] = await _topicRepository.FindAll();
                    return View(post);
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                post.User = user;
                post.DateTime = DateTime.Now;
                post.Topic = await _topicRepository.FindAsyncById(post.TopicId);
                await _postRepository.Create(post);
                return RedirectPermanent("~/Posts/Index");
            }
            return View(post);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postRepository.FirstOrDefaultAsync(id);
            if (post == null)
            {
                _logger.LogError("Doesn't exist post. Controller:Post. Action:Edit. post = null");
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
                   
                    
                    if (post.Image != post1.Image && post1.Image != null && uploadedFile != null && uploadedFile.ContentType.ToLower().Contains("image"))
                    {
                        post1.Image = await _imageService.SaveImageAsync(uploadedFile, 1);
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "Некорректный формат");
                        return View(post1);
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

        public async Task<IActionResult> Delete(int id)
        {
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
