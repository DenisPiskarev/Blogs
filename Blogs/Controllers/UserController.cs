using Blogs.Managers;
using Blogs.Models;
using Blogs.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Blogs.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly BlogManager _blogManager;
        private readonly DraftManager _draftManager;

        public UserController(UserManager<User> userManager, BlogManager blogManager, DraftManager draftManager)
        {
            _userManager = userManager;
            _blogManager = blogManager;
            _draftManager = draftManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Blogs(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var blogs = _blogManager.ListOfUserBlogs(user.Id).OrderByDescending(s => s.ReleaseDate).ToList();
            var userIndexViewModel = new UserIndexViewModel
            {
                Blogs = blogs,
                User = user,
                UserManager = _userManager
            };
            return View(userIndexViewModel);
        }

        public async Task<IActionResult> Drafts(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);

            if (user.Id != currentUser.Id)
            {
                return Forbid();
            }

            var blogs = _draftManager.ListOfUserDrafts(user.Id).OrderByDescending(s => s.dateOfCreation).ToList();
            var userDraftsViewModel = new UserDraftsViewModel
            {
                Drafts = blogs,
                User = user,
                UserManager = _userManager
            };
            return View(userDraftsViewModel);

        }
    }
}
