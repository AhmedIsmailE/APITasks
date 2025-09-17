using API.Core.DTos;
using API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Interfaces
{
    public interface ICategoryServices
    {
        // get all
        Task<IEnumerable<Category>> GetAllAsync();
        // get by id
        Task<Category> GetByIdAsync(int id);
        // create
        Task<Category> CreateAsync(CategoryDTo category); // change parameter datatype
        // update
        Task<bool> UpdateAsync(int id, CategoryDTo category);
        Task<bool> UpdateAsync(CategoryDTo category);
        // delete
        Task<bool> DeleteAsync(int id);
    }
}
