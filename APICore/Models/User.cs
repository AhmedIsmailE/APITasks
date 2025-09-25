using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Models
{
    public class User : IdentityUser
    {
        ////public Guid Id { get; set; }
        //[Key]
        //public int Id { get; set; }
        //[Required(ErrorMessage ="Username is required")]
        //[StringLength(50,MinimumLength =3)]
        //public string Username { get; set; }
        //[Required(ErrorMessage ="Email is required")]
        //[EmailAddress(ErrorMessage ="Invalid email")]
        //public string Email { get; set; }
        //[Required(ErrorMessage = "Password is required")]
        ////[RegularExpression("")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; } // hashed password

        // Relations 
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
