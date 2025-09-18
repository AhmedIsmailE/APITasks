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
    public class CommentDTo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [StringLength(200)] // 1 -> 200
        public string Content { get; set; }

        public int PostId { get; set; }


       
        public int UserId { get; set; }

    }
}
