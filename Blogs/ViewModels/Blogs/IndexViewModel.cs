using Blogs.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blogs.ViewModels.Blog
{
    public class IndexViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public UserManager<User>? userManager { get; set; }
    }
}
