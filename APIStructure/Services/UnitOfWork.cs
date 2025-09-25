using API.Core.Interfaces;
using API.Core.Models;
using API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        // DI For DB for save method and for set every key
        private readonly APIDbContext _context;
        public UnitOfWork(APIDbContext context)
        {
            _context = context;
            Posts = new GenericRepository<Post>(context);
            Comments = new GenericRepository<Comment>(context);
            Categories = new GenericRepository<Category>(context);
            CategoryServices = new CategoryServices(context);
        }
        
        //Keys
        public IGenericRepository<Post> Posts { get; private set; }
        public IGenericRepository<Comment> Comments { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public ICategoryServices CategoryServices { get; private set; }

        public async Task<int> SaveAsync()  // if any error -> return 0... other than that return anything but 0
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
