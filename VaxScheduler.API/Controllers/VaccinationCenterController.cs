using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
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

		[HttpPost("AddCenter")]
		public async Task<ActionResult<UserDTO>> AddVaccinationCenter(VaccinationCenterDTO model)
		{
			if (ModelState.IsValid)
			{
				var EmailFalg = await _dbContext.VaccinationCenters.Where(C => C.Email == model.Email).FirstOrDefaultAsync();
				if (EmailFalg == null)
				{
					var center = new VaccinationCenter
					{
						Name = model.Name,
						Email = model.Email,
						Password = model.Password,
						Role = "Center",
						Location = model.Location,
						AdminId = 1
					};
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
			if(centers?.Count() > 0)
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
	}
}
