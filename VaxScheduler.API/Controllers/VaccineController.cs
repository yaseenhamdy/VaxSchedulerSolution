﻿using Microsoft.AspNetCore.Http;
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
		private readonly IUnitOfWork _unitOfWork;

		public VaccineController(IGenericRepository<Vaccine> vaccineRepo, VaxDbContext dbContext, IUnitOfWork unitOfWork)
		{
			_vaccineRepo = vaccineRepo;
			_dbContext = dbContext;
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Vaccine>>> GetAllVaccine()
		{
			var Vaccines = await _vaccineRepo.GetAllAsync();

			if(Vaccines?.Count() > 0) 
				return Ok(Vaccines);
			else
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
				});

		}



		[HttpPost("AddVaccine")]
		public async Task<ActionResult<AddVaccineResponse>> AddVaccine(AddVaccineDTO model)
		{

			if (ModelState.IsValid)
			{
				var VaccineNameFlag = await _dbContext.Vaccines.Where(V => V.Name == model.Name).FirstOrDefaultAsync();
				if (VaccineNameFlag == null)
				{
					var vaccine = new Vaccine()
					{
						Name = model.Name,
						DurationBetweenDoses = model.DurationBetweenDoses,
						Precautions = model.Precautions,
						AdminId = 1
					};
					await _vaccineRepo.AddAsync(vaccine);
					int Result = await _unitOfWork.Complete();
					if (Result > 0)
					{
						return Ok(new AddVaccineResponse
						{
							Name = model.Name,
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
						});
					}
					

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
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
				});
			}

		}


		[HttpDelete]
		public async Task<ActionResult<StatuseOfResonse>> DeleteVaccine(DeleteCenterDTO model)
		{
			if (ModelState.IsValid)
			{
				var vaccine = await _vaccineRepo.GetByIdAsync(model.Id);
				if (vaccine == null)
					return NotFound(new StatuseOfResonse
					{
						Message = false,
						Value = "Not Found This Vaccine"
					});
				else
				{
					await _vaccineRepo.DeleteAsync(vaccine);
					int Result = await _unitOfWork.Complete();
					if (Result > 0)
					{
						return Ok(new StatuseOfResonse()
						{
							Message = true,
							Value = "Deleted Successfully"

						});
					}
					else
					{
						return Ok(new StatuseOfResonse()
						{
							Message = false,
							Value = "Error"

						});
					}
				}
			}
			else
			{
				return Ok(new StatuseOfResonse()
				{
					Message = false,
					Value = "Error"

				});
			}


		}
	}
}
