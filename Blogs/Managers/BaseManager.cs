using Blogs.Models;

namespace Blogs.Managers
{
    public abstract class BaseManager
    {
        protected readonly ApplicationContext _context;
        protected BaseManager(ApplicationContext context)
        {
            _context = context;
        }
    }
}
