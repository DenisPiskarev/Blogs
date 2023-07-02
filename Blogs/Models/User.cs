using Microsoft.AspNetCore.Identity;

namespace Blogs.Models
{
    public class User : IdentityUser 
    {
        public int Year { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Draft> Drafts { get; set; }

    }
}
