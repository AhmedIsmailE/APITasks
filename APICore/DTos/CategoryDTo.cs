using API.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.DTos
{
    public class CategoryDTo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        public string Name { get; set; }

    }
}
