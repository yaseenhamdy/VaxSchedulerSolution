using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Claims;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Core.Services;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.API.Controllers
{

	public class VaccinationCenterController : APIBaseController
	{
		private readonly VaxDbContext _dbContext;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<VaccinationCenter> _centerRepo;

		private readonly ITokenService _tokenService;
		public VaccinationCenterController(VaxDbContext dbContext, ITokenService tokenService, IUnitOfWork unitOfWork
			, IGenericRepository<VaccinationCenter> centerRepo)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
			_unitOfWork = unitOfWork;
			_centerRepo = centerRepo;

		}

		[HttpGet("some-action")]
		public IActionResult SomeAction()
		{
			var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
			if (userRole != "Admin")
			{
				return Unauthorized("You do not have permission to view this resource.");
			}
			return Ok("Access granted to protected resource.");
		}


		[HttpPost("AddCenter")]
		[Authorize(Roles = "Admin")]

		public async Task<ActionResult<UserDTO>> AddVaccinationCenter(VaccinationCenterDTO model)
		{
			if (ModelState.IsValid)
			{
				var EmailFalg = await _dbContext.VaccinationCenters.Where(C => C.Email == model.Email).FirstOrDefaultAsync();
				if (EmailFalg == null)
				{
					var AdminId = _dbContext.Admins.FirstOrDefault();
					var center = new VaccinationCenter
					{
						Name = model.Name,
						Email = model.Email,
						Password = model.Password,
						Role = "Center",
						Location = model.Location,
						AdminId = AdminId.Id
					};
					var hasher = new PasswordHasher<VaccinationCenter>();
					center.Password = hasher.HashPassword(center, model.Password);
					await _centerRepo.AddAsync(center);
					int Result = await _unitOfWork.Complete();
					if (Result > 0)
					{
						return Ok(new UserDTO()
						{
							Name = model.Name,
							Email = model.Email,
							Role = "Center",
							Token = await _tokenService.CreateAdminTokenAsync(center),
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
							Value = "Email Alrady Exist"
						});

					}

				}
				else
				{
					return BadRequest(new StatuseOfResonse
					{
						Message = false,
						Value = "Email Alrady Exist"
					});
				}



			}
			else
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "Email Alrady Exist"
				});

		}



		[HttpGet]
		public async Task<ActionResult<List<GetAllCenter>>> GetAllVaccinationCenter()
		{
			var centers = await _centerRepo.GetAllAsync();
			if (centers?.Count() > 0)
			{
				var centerDtos = centers.Select(center => new GetAllCenter
				{
					Id = center.Id,
					Name = center.Name,
					Email = center.Email,
					Location = center.Location,
					Role = center.Role,
					VaccineNames = center.VaccineVaccinationCenter.Select(vvc => new VaccineVaccinationCenterDto
					{
						VaccineId = vvc.Vaccine.Id, 
						VaccineName = vvc.Vaccine.Name
					}).ToList()
				}).ToList();

				return Ok(centerDtos);
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



		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<StatuseOfResonse>> DeleteVaccinationCenter(DeleteCenterDTO model)
		{
			if (ModelState.IsValid)
			{
				var center = await _centerRepo.GetByIdAsync(model.Id);
				if (center == null)
					return NotFound(new StatuseOfResonse
					{
						Message = false,
						Value = "Not Found This Vaccination Center"
					});
				else
				{
					await _centerRepo.DeleteAsync(center);
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
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<UserDTO>> UpdateVaccineCenter(int id, VaccinationCenterDTO model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "Invalid model data."
				});
			}

			var vCenter = await _centerRepo.GetByIdAsync(id);
			if (vCenter == null)
			{
				return NotFound(new StatuseOfResonse
				{
					Message = false,
					Value = "Vaccination Center not found."
				});
			}

			var existingCenterWithEmail = await _dbContext.VaccinationCenters
				.Where(c => c.Email == model.Email && c.Id != id)
				.FirstOrDefaultAsync();

			if (existingCenterWithEmail != null)
			{
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "Email already exists with another center."
				});
			}

			vCenter.Name = model.Name;
			vCenter.Email = model.Email;
			vCenter.Location = model.Location;

			if (!string.IsNullOrEmpty(model.Password))
			{
				var hasher = new PasswordHasher<VaccinationCenter>();
				vCenter.Password = hasher.HashPassword(vCenter, model.Password);
			}

			await _centerRepo.UpdateAsync(vCenter);
			int result = await _unitOfWork.Complete();
			if (result > 0)
			{
				return StatusCode(500, new StatuseOfResonse
				{
					Message = false,
					Value = "An error occurred while updating the center."
				});
			}

			return Ok(new UserDTO
			{
				Name = vCenter.Name,
				Email = vCenter.Email,
				Role = "Center",
				Status = new StatuseOfResonse
				{
					Message = true,
					Value = "Success"
				}
			});
		}





		[HttpGet("{id}")]
		public async Task<ActionResult<List<GetAllCenter>>> GetVaccinationCenterById(int id)
		{
			var center = await _centerRepo.GetByIdAsync(id);
			if (center is not null)
			{

				var centerDtos = new GetAllCenter
				{
					Id = center.Id,
					Name = center.Name,
					Email = center.Email,
					Location = center.Location,
					Role = center.Role,
					VaccineNames = center.VaccineVaccinationCenter.Select(vvc => new VaccineVaccinationCenterDto
					{
						VaccineName = vvc.Vaccine.Name
					}).ToList()
				};

				return Ok(centerDtos);
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







		[HttpPost("ApproveDoses")]
		public async Task<ActionResult<StatuseOfResonse>> ApproveDoses(SendDoseDTo model)
		{
			var patientVaccines = await _dbContext.patientVaccines
								   .Where(PV => PV.PatientId == model.PatientId && PV.VaccineId == model.VaccineId)
								   .FirstOrDefaultAsync();
			if (patientVaccines is not null)
			{
				if (patientVaccines.FlagFirstDose == 0)
				{
					patientVaccines.FirstDose = 1;
					patientVaccines.FlagFirstDose = 1;
					_dbContext.patientVaccines.Update(patientVaccines);
					int result = await _unitOfWork.Complete();
					if (result > 0)
					{
						return Ok(new StatuseOfResonse
						{
							Message = true,
							Value = "First Dose Approved Successfully"
						});
					}
					else
					{
						return BadRequest(new StatuseOfResonse
						{
							Message = false,
							Value = "No Rows Affected"
						});
					}
				}
				else if (patientVaccines.FlagSecondDose == 0)
				{
					patientVaccines.FlagSecondDose = 1;
					patientVaccines.SecondDose = 1;
					_dbContext.patientVaccines.Update(patientVaccines);
					int result = await _unitOfWork.Complete();
					if (result > 0)
					{
						return Ok(new StatuseOfResonse
						{
							Message = true,
							Value = "Second Dose Approved Successfully"
						});
					}
					else
					{
						return BadRequest(new StatuseOfResonse
						{
							Message = false,
							Value = "No Rows Affected"
						});
					}
				}
				else
				{
					return BadRequest(new StatuseOfResonse
					{
						Message = false,
						Value = "You Take Two Doses from this Vaccine not Allow To TAke More"
					});
				}
			}
			else
			{

				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "There Are No Request With Patient ID or Vaccine ID or Vaccinatio Center ID Like This "
				});

			}

		}





		[HttpGet("GetAllWaitingDoses/{vaccinationCenterID}")]
		public async Task<ActionResult<List<GetAllWaitingDosesDTO>>> GetAllWaitingDoses(int vaccinationCenterID)
		{

			var result = await _dbContext.patientVaccines
		   .Where(pv => (pv.VaccinationCenterId == vaccinationCenterID) && (pv.FlagFirstDose == 0 || pv.FlagSecondDose == 0))
		   .Select(pv => new GetAllWaitingDosesDTO
		   {
			   PatientId = pv.PatientId,
			   Name = pv.Patient.Name,
			   Email = pv.Patient.Email,
			   VaccineId = pv.VaccineId,
			   VaccineName = pv.Vaccine.Name,
		   })
		   .Distinct()
		   .ToListAsync();

			if (result.Count() == 0)
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "No waiting doses found"
				});

			return Ok(result);


		}



	}
}
