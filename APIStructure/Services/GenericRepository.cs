using API.Core.Interfaces;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Services
{
    public class GenericRepository<Table> : IGenericRepository<Table> where Table : class
    {
        // DI for DbContext
        private readonly APIDbContext _context;
        private readonly DbSet<Table> _table;
        public GenericRepository(APIDbContext context)
        {
            _context = context;
            _table = _context.Set<Table>();
        }

        //public async Task<IEnumerable<Table>> GetAllAsync()
        //{
        //    return await _table.ToListAsync();
        //}

        public async Task<Table> GetByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }
        public async Task CreateAsync(Table model)
        {
            await _table.AddAsync(model);
        }
        //public async Task SaveAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}

        public void Update(Table model)
        {
            _table.Update(model);
        }

        public void Delete(Table model)
        {
            _table.Remove(model);
        }

        public async Task<IEnumerable<Table>> GetAllAsync(
            Expression<Func<Table, bool>> predicate = null,
            IEnumerable<Expression<Func<Table, object>>> includes = null)
        {
            // select * from table
            IQueryable<Table> query = _table;
            // select * from table where (condition)
            if (predicate is not null)
            {
                query = query.Where(predicate);
            }
            // handle any includes (sub queries)
            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }
            return await query.ToListAsync();
        }
    }
}
