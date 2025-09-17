using API.Core.DTos;
using API.Core.Interfaces;
using API.Core.Models;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    public class CategoryServices : ICategoryServices
    {   
        // dependancy injection use the ability of one class in another one
        private readonly APIDbContext _context;
        public CategoryServices(APIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var Categories = await _context.Categories.ToListAsync();
            //var Categories = _context.Categories.Select(c => new Category
            //{
            //    Name = c.Name,
            //    Id = c.Id,
            //    //  Posts = c.Posts

            //});

            return Categories;
        }

        public Task<Category> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> CreateAsync(CategoryDTo category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, CategoryDTo category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CategoryDTo category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
