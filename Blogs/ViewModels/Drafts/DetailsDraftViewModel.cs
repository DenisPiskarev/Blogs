using Blogs.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Blogs.ViewModels.Draft
{
    public class DetailsDraftViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime dateOfCreation { get; set; }
        public string Category { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public UserManager<User>? userManager { get; set; }
    }
}
