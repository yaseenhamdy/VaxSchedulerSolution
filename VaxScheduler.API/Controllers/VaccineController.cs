using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

			if (Vaccines?.Count() > 0)
			{
				var vaccineDTO = Vaccines.Select(vaccine => new GetAllVaccineDTO
				{
					Id = vaccine.Id,
					Name = vaccine.Name,
					Precautions = vaccine.Precautions,
					DurationBetweenDoses = vaccine.DurationBetweenDoses,
					VaccinationCenterName = vaccine.VaccineVaccinationCenter.Select(v => new VaccineCenterVaccineInVaccineDTO
					{
						VaccinationCenterName = v.VaccinationCenter.Name
					}).ToList()
				}).ToList();

				return Ok(vaccineDTO);
			}
			else
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "There Are No Vaccines"
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
						AdminId = 2,
					};
					await _vaccineRepo.AddAsync(vaccine);
					int Result = await _unitOfWork.Complete();
					if (Result > 0)
					{
						foreach (var vaccinationCenterId in model.VaccinationCenterIds)
						{
							var vaccineVaccinationCenter = new VaccineVaccinationCenter
							{
								VaccineId = vaccine.Id,
								VaccinationCenterId = vaccinationCenterId
							};

							_dbContext.vaccineVaccinationCenters.Add(vaccineVaccinationCenter);
						}

						await _unitOfWork.Complete();

						var vaccinationCenters = await _dbContext.VaccinationCenters
	                                            	.Where(vc => model.VaccinationCenterIds.Contains(vc.Id))
	                                               	.ToListAsync();

						var vaccinationCenterNames = vaccinationCenters.Select(vc => vc.Name).ToArray();
						return Ok(new AddVaccineResponse
						{
							Name = model.Name,
							VaccinationCenterNames = vaccinationCenterNames,
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






		[HttpPut("{id}")]
		public async Task<ActionResult<UserDTO>> UpdateVaccine(int id, AddVaccineDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "Invalid model data."
				});
			}

			var vaccine = await _vaccineRepo.GetByIdAsync(id);
			if (vaccine == null)
			{
				return NotFound(new StatuseOfResonse
				{
					Message = false,
					Value = "Vaccine not found."
				});
			}

			var existingCenterWithEmail = await _dbContext.Vaccines
				.Where(c => c.Name == model.Name && c.Id != id)
				.FirstOrDefaultAsync();

			if (existingCenterWithEmail != null)
			{
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "Email already exists with another center."
				});
			}

			vaccine.Name = model.Name;
			vaccine.Precautions = model.Precautions;
			vaccine.DurationBetweenDoses = model.DurationBetweenDoses;


			await _vaccineRepo.UpdateAsync(vaccine);
			int result = await _unitOfWork.Complete();
			if (result > 0)
			{
				return StatusCode(500, new StatuseOfResonse
				{
					Message = false,
					Value = "An error occurred while updating the vaccine."
				});
			}

			return Ok(new UpdateVaccineResponse
			{
				Name = vaccine.Name,
				Precautions = vaccine.Precautions,
				DurationBetweenDoses = vaccine.DurationBetweenDoses,
				Status = new StatuseOfResonse
				{
					Message = true,
					Value = "Success"
				}
			});
		}





		[HttpGet("{id}")]
		public async Task<ActionResult<GetVaccineByIdDTO>> GetVaccinationCenterById(int id)
		{
			var vaccine = await _vaccineRepo.GetByIdAsync(id);
			if (vaccine is not null)
			{

				var vaccineDTO = new GetAllVaccineDTO
				{
					Id = vaccine.Id,
					Name = vaccine.Name,
					Precautions = vaccine.Precautions,
					DurationBetweenDoses = vaccine.DurationBetweenDoses,
					VaccinationCenterName = vaccine.VaccineVaccinationCenter.Select(v => new VaccineCenterVaccineInVaccineDTO
					{
						VaccinationCenterName = v.VaccinationCenter.Name
					}).ToList()
				};

				return Ok(vaccineDTO);

			}
			else
			{
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "There Are No Vaccination Centers "
				});
			}

		}

	}
}
