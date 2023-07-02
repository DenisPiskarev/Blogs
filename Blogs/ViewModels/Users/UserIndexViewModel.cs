using Blogs.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blogs.ViewModels.Users
{
    public class UserIndexViewModel
    {
        public List<Models.Blog> Blogs { get; set; }
        public virtual User User { get; set; }
        public UserManager<User>? UserManager { get; set; }
    }
}
