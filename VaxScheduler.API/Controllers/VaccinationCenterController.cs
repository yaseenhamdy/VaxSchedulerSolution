using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
using VaxScheduler.Core.Services;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.API.Controllers
{

	public class VaccinationCenterController : APIBaseController
	{
		private readonly VaxDbContext _dbContext;
		private readonly ITokenService _tokenService;
		public VaccinationCenterController(VaxDbContext dbContext, ITokenService tokenService)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
		}

		[HttpPost("AddCenter")]
		public async Task<ActionResult<UserDTO>> AddVaccinationCenter(VaccinationCenterDTO model)
		{
			if (ModelState.IsValid)
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
				var EmailFalg = await _dbContext.VaccinationCenters.Where(C => C.Email == model.Email).FirstOrDefaultAsync();
				if (EmailFalg == null)
				{
					await _dbContext.VaccinationCenters.AddAsync(center);
					await _dbContext.SaveChangesAsync();
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
				return BadRequest();

		}
	}
}
