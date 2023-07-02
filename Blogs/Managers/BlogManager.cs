using Blogs.Models;
using Microsoft.Extensions.Hosting;

namespace Blogs.Managers
{
    public class BlogManager : BaseManager
    {
        public BlogManager(ApplicationContext context) : base(context)
        {

        }
        public List<Blog> List()
        {
            return _context.Blogs.ToList();
        }
        public List<Blog> ListOfUserBlogs(string userId)
        {
            return _context.Blogs.Where(u => u.UserId == userId).ToList();
        }

        public void Add(Blog post)
        {
            _context.Blogs.Add(post);
            _context.SaveChanges();
        }

        public void Update(Blog post)
        {
            _context.Blogs.Update(post);
            _context.SaveChanges();
        }

        public Blog GetBlog(int id)
        {
            return _context.Blogs.Find(id);
        }

        public void Delete(Blog post)
        {
            _context.Blogs.Remove(post);
            _context.SaveChanges();
        }

        public bool ExistsBlog(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }

    }
}
