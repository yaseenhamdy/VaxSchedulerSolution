using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Interfaces;

namespace VaxScheduler.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
		Task AddAsync(T entity) ;
		Task DeleteAsync<T>(T entity);
		Task UpdateAsync(T entity);

	}
}
