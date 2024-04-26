using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly VaxDbContext _dbContext;

		public UnitOfWork(VaxDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<int> Complete()
		=> await _dbContext.SaveChangesAsync();

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
