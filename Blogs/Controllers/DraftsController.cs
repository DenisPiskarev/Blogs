using Blogs.Managers;
using Blogs.Models;
using Blogs.ViewModels.Draft;
using Blogs.ViewModels.Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Blogs.Controllers
{
    [Authorize]
    public class DraftsController : Controller
    {
        private readonly DraftManager _draftManager;
        private readonly UserManager<User> _userManager;
        public DraftsController(UserManager<User> userManager, DraftManager draftManager)
        {
            _userManager = userManager;
            _draftManager = draftManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDraft(AddViewModel AddViewModel)
        {
            var post = new Draft
            {
                Category = AddViewModel.Category,
                Title = AddViewModel.Title,
                Text = AddViewModel.Text,
                dateOfCreation = DateTime.Now,
                UserId = _userManager.GetUserId(User)
            };
            _draftManager.AddDraft(post);
            return RedirectToAction("Index", "Blogs");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = _draftManager.GetDraft((int)id);
            if (post == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);

            if (post.UserId != currentUser.Id)
            {
                return Forbid();
            }

            var detailsViewModel = new DetailsDraftViewModel
            {
                Id = post.Id,
                Title = post.Title,
                dateOfCreation = post.dateOfCreation,
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

            var post = _draftManager.GetDraft((int)id);
            if (post == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (post.UserId != currentUser.Id)
            {
                return Forbid();
            }

            var editViewModel = new EditDraftViewModel
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
        public IActionResult Edit(int id, EditDraftViewModel editViewModel)
        {
            if (id != editViewModel.Id)
            {
                return NotFound();
            }
            var post = _draftManager.GetDraft(id);
            post.Title = editViewModel.Title;
            post.Category = editViewModel.Category;
            post.Text = editViewModel.Text;
                try
                {
                    _draftManager.UpdateDraft(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_draftManager.ExistsDraft(post.Id))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _draftManager.GetDraft(id);
            _draftManager.DeleteDraft(post);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
