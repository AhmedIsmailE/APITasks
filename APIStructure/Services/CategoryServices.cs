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
            var Categories = await _context.Categories
                .Include(c => c.Posts)     // Left join
                .ThenInclude(p => p.User) // subquery with inner join
                .ToListAsync();
            //var Categories = _context.Categories.Select(c => new Category
            //{
            //    Name = c.Name,
            //    Id = c.Id,
            //    //  Posts = c.Posts

            //});

            return Categories;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id); // <----- best for primary key search
            //return await _context.Categories.FirstAsync(c => c.Id == id);
            //return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id); <----- finidng based on not primary key
            //return await _context.Categories.SingleAsync(c => c.Id == id);
            //return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            // Explicit loading:
            //var Category = await _context.Categories.FindAsync(id);
            //await _context.Entry(Category).Collection(c => c.Posts).LoadAsync();
            //return Category;
        }

        public async Task<Category> CreateAsync(CategoryDTo category)
        {
            var Category = new Category
            {
                Name = category.Name,
                //Posts = category.Posts
            };
            await _context.Categories.AddAsync(Category);
            await _context.SaveChangesAsync();
            return Category;
        }

        public async Task<bool> UpdateAsync(int id, CategoryDTo category)
        {
            //var ExistingCategory = await _context.Categories.FindAsync(id);
            var ExistingCategory = await GetByIdAsync(id);
            if (ExistingCategory == null)
            {
                return false;
            }
            ExistingCategory.Name = category.Name;
            _context.Categories.Update(ExistingCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CategoryDTo category)
        {
            var ExistingCategory = await GetByIdAsync(category.Id);
            if (ExistingCategory == null)
            {
                return false;
            }
            ExistingCategory.Name = category.Name;
            _context.Categories.Update(ExistingCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ExistingCategory = await GetByIdAsync(id);
            if (ExistingCategory == null)
            {
                return false;
            }
            _context.Categories.Remove(ExistingCategory);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
