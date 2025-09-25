using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(150,MinimumLength =4)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [StringLength(400, MinimumLength = 20)]
        public string Content { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Relations
        // userid
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        // categoryid
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();   
    }
}
