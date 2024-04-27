using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Services;
using VaxScheduler.Repository.Data;
using Microsoft.AspNetCore.Identity; 


namespace VaxScheduler.API.Controllers
{
	public class AccountsController : APIBaseController
	{
		private readonly VaxDbContext _dbContext;
		private readonly ITokenService _tokenService;

		public AccountsController(VaxDbContext dbContext, ITokenService tokenService)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
		{
			if (ModelState.IsValid)
			{
				var patient = new Patient
				{
					Name = model.Name,
					Email = model.Email,
					Password = model.Password,
					Role = "Patient",
					Ssn = model.Ssn,
					Phone = model.Phone,
					AdminId = 2

				};

				var hasher = new PasswordHasher<Patient>();
				patient.Password = hasher.HashPassword(patient, model.Password);

				if (int.TryParse(model.VaccinationCenterId.ToString(), out int vaccinationCenterId)) // Make sure to convert to string
				{
					patient.VaccinationCenterId = vaccinationCenterId;
				}
				else
				{
					ModelState.AddModelError("VaccinationCenterId", "Invalid Vaccination Center ID");
					return BadRequest(ModelState);
				}
				var EmailFalg = await _dbContext.Patients.Where(P => P.Email == model.Email).FirstOrDefaultAsync();
				if (EmailFalg == null)
				{
					await _dbContext.Patients.AddAsync(patient);
					await _dbContext.SaveChangesAsync();
					return Ok(new UserDTO()
					{
						Name = model.Name,
						Email = model.Email,
						Role = "Patient",
						Token = await _tokenService.CreateAdminTokenAsync(patient),
						Status = new StatuseOfResonse ()
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

		[HttpPost("Login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
		{
			if (ModelState.IsValid)
			{
				var Admin = await _dbContext.Admins.Where(A => A.Email == model.Email ).FirstOrDefaultAsync();
				var Patient = await _dbContext.Patients.Where(P => P.Email == model.Email ).FirstOrDefaultAsync();
				var Center = await _dbContext.VaccinationCenters.Where(C => C.Email == model.Email).FirstOrDefaultAsync();
				if (Admin is not null)
				{
					var hasher = new PasswordHasher<Admin>();
					var verificationResult = hasher.VerifyHashedPassword(Admin, Admin.Password, model.Password);

					if (verificationResult == PasswordVerificationResult.Success)
					{
						return Ok(new UserDTO()
						{
							Name = Admin.Name,
							Email = Admin.Email,
							Role = Admin.Role,
							Token = await _tokenService.CreateAdminTokenAsync(Admin),
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
							Value = "Wrong Password"
						});
					}

				}
				else if(Patient is not null)
				{
					var hasher = new PasswordHasher<Patient>();
					var verificationResult = hasher.VerifyHashedPassword(Patient, Patient.Password, model.Password);

					if (verificationResult== PasswordVerificationResult.Success)
					{
						return Ok(new UserDTO()
						{
							Name = Patient.Name,
							Email = Patient.Email,
							Role = Patient.Role,
							Token = await _tokenService.CreateAdminTokenAsync(Patient),
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
							Value = "Wrong Password"
						});
					}
					
				}
				else if (Center is not null)
				{
					var hasher = new PasswordHasher<VaccinationCenter>();
					var verificationResult = hasher.VerifyHashedPassword(Center, Center.Password, model.Password);

					if (verificationResult == PasswordVerificationResult.Success)
					{
						return Ok(new UserDTO()
						{
							Name = Center.Name,
							Email = Center.Email,
							Role = Center.Role,
							Token = await _tokenService.CreateAdminTokenAsync(Center),
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
							Value = "Wrong Password"
						});
					}
				}
				else
				{
					return BadRequest(new StatuseOfResonse
					{ 

						Message = false,
						Value = "Invalid Email or Password"
					});
				}
			}
			else { return BadRequest(); }

		}




		//[HttpPost("AddAdmin")]
		//public async Task<ActionResult<UserDTO>> AddAdmin(AdminDTO model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var admin = new Admin
		//		{
		//			Name = model.Name,
		//			Email = model.Email,
		//			Password = model.Password,
		//			Role = "Admin",
		//		};
		//		var hasher = new PasswordHasher<Admin>();
		//		admin.Password = hasher.HashPassword(admin, model.Password);
		//		await _dbContext.AddAsync(admin);
		//		int Result = await _dbContext.SaveChangesAsync();
		//		if (Result > 0)
		//		{
		//			return Ok(new UserDTO()
		//			{
		//				Name = model.Name,
		//				Email = model.Email,
		//				Role = "Admin",
		//				Token = await _tokenService.CreateAdminTokenAsync(admin),
		//				Status = new StatuseOfResonse()
		//				{
		//					Message = true,
		//					Value = "Success"

		//				}

		//			});
		//		}
		//		else
		//		{
		//			return BadRequest(new StatuseOfResonse
		//			{
		//				Message = false,
		//				Value = "Email Alrady Exist"
		//			});

		//		}





		//	}
		//	else
		//		return BadRequest(new StatuseOfResonse
		//		{
		//			Message = false,
		//			Value = "Email Alrady Exist"
		//		});

		//}

	}
}
