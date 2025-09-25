using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Interfaces
{
    public interface IGenericRepository<Table> where Table : class
    {
        //Task<IEnumerable<Table>> GetAllAsync();
        Task<IEnumerable<Table>> GetAllAsync(
                Expression<Func<Table,bool>> predicate = null, // predicate => where statement
                IEnumerable<Expression<Func<Table,object>>> includes = null
            );
        Task<Table> GetByIdAsync(int id);
        Task CreateAsync(Table model);
        //Task SaveAsync();
        void Update(Table model);
        void Delete(Table model);
    }
}
