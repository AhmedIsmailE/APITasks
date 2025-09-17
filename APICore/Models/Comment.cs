using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [StringLength(200)] // 1 -> 200
        public string Content { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Relations 

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
