using API.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable // give authority to erase anything
    {

        IGenericRepository<Post> Posts { get; }

        IGenericRepository<Comment> Comments { get; }

        IGenericRepository<Category> Categories { get; }

        ICategoryServices CategoryServices { get; }

        Task<int> SaveAsync();
    }
}
