using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogs.Models
{
    public class Blog
    {
      public int Id { get; set; }
      public string Title { get; set; }

      [DataType(DataType.Date)]
      public DateTime ReleaseDate { get; set; }
      public string Category { get; set; }
      public string Text { get; set; }

      [ForeignKey("User")]
      public string UserId { get; set; }
      public virtual User User { get; set; }
    }
}
