using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.API.Controllers
{

	public class VaccineController : APIBaseController
	{
		private readonly IGenericRepository<Vaccine> _vaccineRepo;
		private readonly VaxDbContext _dbContext;

		public VaccineController(IGenericRepository<Vaccine> vaccineRepo, VaxDbContext dbContext)
		{
			_vaccineRepo = vaccineRepo;
			_dbContext = dbContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Vaccine>>> GetAllVaccine()
		{
			var Vaccines = await _vaccineRepo.GetAllAsync();

			var vaccineDTOs = Vaccines.Select(v => new VaccineDTO
			{
				Id = v.Id,
				Name = v.Name,
				Precautions = v.Precautions,
				DurationBetweenDoses = v.DurationBetweenDoses,
				Status = new StatuseOfResonse()
				{
					Message = true,
					Value = "Success"

				}
			}).ToList();
			return Ok(vaccineDTOs);

		}



		[HttpPost("AddVaccine")]
		public async Task<ActionResult<AddVaccineResponse>> AddVaccine(AddVaccineDTO model)
		{

			if (ModelState.IsValid)
			{
				var VaccineNameFlag =await _dbContext.Vaccines.Where(V => V.Name == model.Name).FirstOrDefaultAsync();
				if (VaccineNameFlag == null)
				{
					var vaccine = new Vaccine()
					{
						Name = model.Name,
						DurationBetweenDoses = model.DurationBetweenDoses,
						Precautions = model.Precautions,
						AdminId = 1
					};
					await _dbContext.Vaccines.AddAsync(vaccine);
					await _dbContext.SaveChangesAsync();

					return Ok(new AddVaccineResponse
						{
							Name= model.Name,
							Status = new StatuseOfResonse()
							{
								Message = true,
								Value = "Success"

							}
						});

				}
				else
				{

					return BadRequest(new StatuseOfResonse
					{
						Message = false,
						Value = "Vaccine Alrady Exist"
					});

				}

			}
			else
			{
				return BadRequest();	
			}

		}
	}
}
