using Microsoft.AspNetCore.Mvc;
using Blogs.Models;
using Blogs.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blogs.ViewModels.Blog;
using Microsoft.AspNetCore.Authorization;

namespace Blogs.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly BlogManager _blogManager;
        private readonly UserManager<User> _userManager;
        public BlogsController(UserManager<User> userManager, BlogManager blogManager)
        {
            _userManager = userManager;
            _blogManager = blogManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var posts = _blogManager.List().OrderByDescending(s => s.ReleaseDate).ToList();
            var indexViewModels = posts.Select(p => new IndexViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ReleaseDate = p.ReleaseDate,
                Category = p.Category,
                Text = p.Text,
                UserId = p.UserId,
                User = p.User,
                userManager = _userManager
            }).ToList();
            return View(indexViewModels);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddViewModel AddViewModel)
        {
            if (ModelState.IsValid)
            {
                var post = new Blog
                {
                    Category = AddViewModel.Category,
                    Title = AddViewModel.Title,
                    Text = AddViewModel.Text,
                    ReleaseDate = DateTime.Now,
                    UserId = _userManager.GetUserId(User)
            };
                _blogManager.Add(post);
                return RedirectToAction("Index", "Blogs");
            }
            return View(AddViewModel);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _blogManager.GetBlog((int)id);
            if (post == null)
            {
                return NotFound();
            }
            var detailsViewModel = new DetailsViewModel
            {
                Id = post.Id,
                Title = post.Title,
                ReleaseDate = post.ReleaseDate,
                Category = post.Category,
                Text = post.Text,
                UserId = post.UserId,
                User = post.User,
                userManager = _userManager
            };
            return View(detailsViewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _blogManager.GetBlog((int)id);
            if (post == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            // Проверяем, принадлежит ли блог текущему пользователю
            if (post.UserId != currentUser.Id)
            {
                // Если блог не принадлежит текущему пользователю, возвращаем ошибку
                return Forbid();
            }
            var editViewModel = new EditViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Category = post.Category,
                Text = post.Text
            };
            return View(editViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditViewModel editViewModel)
        {
            if (id != editViewModel.Id)
            {
                return NotFound();
            }
            var post = _blogManager.GetBlog(id);
            post.Title = editViewModel.Title;
            post.Category = editViewModel.Category;
            post.Text = editViewModel.Text;
            if (ModelState.IsValid)
            {
                try
                {
                    _blogManager.Update(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_blogManager.ExistsBlog(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Blogs");
            }
            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _blogManager.GetBlog(id);
            _blogManager.Delete(post);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
