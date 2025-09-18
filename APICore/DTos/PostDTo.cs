using API.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.DTos
{
    public class PostDTo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, MinimumLength = 4)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [StringLength(400, MinimumLength = 20)]
        public string Content { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }

    }
}
