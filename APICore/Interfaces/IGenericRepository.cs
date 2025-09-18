using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Interfaces
{
    public interface IGenericRepository<Table> where Table : class
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table> GetByIdAsync(int id);
        Task CreateAsync(Table model);
        Task SaveAsync();
        void Update(Table model);
        void Delete(Table model);
    }
}
