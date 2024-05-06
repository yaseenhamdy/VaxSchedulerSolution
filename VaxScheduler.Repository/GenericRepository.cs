using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Interfaces;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly VaxDbContext _dbContext;

		public GenericRepository(VaxDbContext dbContext)
		 => _dbContext = dbContext;

		public async Task AddAsync(T entity)
			=> await _dbContext.Set<T>().AddAsync(entity);


		public async Task DeleteAsync<T>(T entity)
			=> _dbContext.Remove(entity);



		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if (typeof(T) == typeof(Vaccine))
			{
				return (IEnumerable<T>)await _dbContext.Vaccines
														.Include(v => v.VaccineVaccinationCenter)
														.ThenInclude(vvc => vvc.VaccinationCenter)
														.ToListAsync();

			}
			else if (typeof(T) == typeof(VaccinationCenter))
			{
				return (IEnumerable<T>)await _dbContext.VaccinationCenters
													 .Include(c => c.VaccineVaccinationCenter)
														.ThenInclude(vvc => vvc.Vaccine)
														.ToListAsync();

			}
			else if (typeof(T) == typeof(VaccinationCenter))
			{
				return (IEnumerable<T>)await _dbContext.Admins.ToListAsync();

			}
			else if (typeof(T) == typeof(RegisteredPatient))
			{
				return (IEnumerable<T>)await _dbContext.RegisteredPatients.ToListAsync();

			}
			else if (typeof(T) == typeof(Certificate))
			{
				return (IEnumerable<T>)await _dbContext.Certificates
																	 .Include(c => c.Vaccine)
																		.Include(cc => cc.Patient)
																		.Include(c => c.VaccinationCenter)
																		.ToListAsync();
			}
			else
			{
				return await _dbContext.Set<T>().ToListAsync();

			}

		}

		public async Task<T> GetByIdAsync(int id)
		{
			if (typeof(T) == typeof(VaccinationCenter))
			{
				return (T)(object)await _dbContext.VaccinationCenters.Include(c => c.VaccineVaccinationCenter)
														.ThenInclude(vvc => vvc.Vaccine)
														.SingleOrDefaultAsync(c => c.Id == id);

			}
			else if (typeof(T) == typeof(Vaccine))
			{
				return (T)(object)await _dbContext.Vaccines.Include(v => v.VaccineVaccinationCenter)
														.ThenInclude(vvc => vvc.VaccinationCenter)
														.SingleOrDefaultAsync(v => v.Id == id);
			}
			else if (typeof(T) == typeof(Patient))
			{
				return (T)(object)await _dbContext.Patients
		.Where(p => p.Id == id)
		.Include(p => p.patientVaccines)
			.ThenInclude(pv => pv.Vaccine)
		.Include(p => p.patientVaccines)
			.ThenInclude(pv => pv.VaccinationCenter)
		.FirstOrDefaultAsync();
			}
			return await _dbContext.Set<T>().FindAsync(id);

		}

		public async Task UpdateAsync(T entity)
		{
			_dbContext.Update(entity);
			await _dbContext.SaveChangesAsync();

		}
	}
}
