using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Category Name is required")]
        public string Name { get; set; }

        // Relations
        public ICollection<Post> Posts { get; set; } = new List<Post>();

    }
}
