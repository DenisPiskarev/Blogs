using Blogs.Models;

namespace Blogs.Managers
{
    public class DraftManager : BaseManager
    {
        public DraftManager(ApplicationContext context) : base(context)
        {

        }
        public List<Draft> ListOfUserDrafts(string userId)
        {
            return _context.Drafts.Where(u => u.UserId == userId).ToList();
        }
        public void AddDraft(Draft post)
        {
            _context.Drafts.Add(post);
            _context.SaveChanges();
        }
        public void UpdateDraft(Draft post)
        {
            _context.Drafts.Update(post);
            _context.SaveChanges();
        }
        public Draft GetDraft(int id)
        {
            return _context.Drafts.Find(id);
        }
        public void DeleteDraft(Draft post)
        {
            _context.Drafts.Remove(post);
            _context.SaveChanges();
        }
        public bool ExistsDraft(int id)
        {
            return _context.Drafts.Any(e => e.Id == id);
        }
    }
}
