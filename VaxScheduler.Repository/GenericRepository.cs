using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly VaxDbContext _dbContext;

		public GenericRepository(VaxDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<int> Add(T entity)
		{

			await _dbContext.Set<T>().AddAsync(entity);
			return await _dbContext.SaveChangesAsync();


		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if (typeof(T) == typeof(Vaccine))
			{
				return (IEnumerable<T>)await _dbContext.Vaccines.ToListAsync();

			}
			else
			{
				return await _dbContext.Set<T>().ToListAsync();

			}

		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}
	}
}
