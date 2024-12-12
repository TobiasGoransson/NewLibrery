using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Interfaces.RepoInterfaces
{
    public interface IRepository<T>
    {
        Task<T> CreateAsync(T entity);

        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<List<T>> GetAllAsync();

        Task<string> DeleteByIdAsync(int id);

        Task UpdateAsync( T entity, CancellationToken cancellationToken);

        
    }

}
