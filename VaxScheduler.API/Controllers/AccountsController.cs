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
using VaxScheduler.Core.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.PortableExecutable;


namespace VaxScheduler.API.Controllers
{
	public class AccountsController : APIBaseController
	{
		private readonly VaxDbContext _dbContext;
		private readonly ITokenService _tokenService;
		private readonly IGenericRepository<Patient> _accRepo;
		private readonly IGenericRepository<RegisteredPatient> _rAccRepo;
		private readonly IUnitOfWork _unitOfWork;

		public AccountsController(VaxDbContext dbContext, ITokenService tokenService
			, IGenericRepository<Patient> accRepo, IGenericRepository<RegisteredPatient> rAccRepo,
			IUnitOfWork unitOfWork)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
			_accRepo = accRepo;
			_rAccRepo = rAccRepo;
			_unitOfWork = unitOfWork;
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

		[Authorize(Roles = "Admin")]
		[HttpPost("AcceptUser")]
		public async Task<ActionResult<UserDTO>> AcceptUser(RegisterDTO model)
		{
			if (ModelState.IsValid)
			{
				var AdminId = _dbContext.Admins.FirstOrDefault();
				var patient = new Patient
				{
					Name = model.Name,
					Email = model.Email,
					Password = model.Password,
					Role = "Patient",
					Ssn = model.Ssn,
					Phone = model.Phone,
					AdminId = AdminId.Id
				};
				var hasher = new PasswordHasher<Patient>();
				patient.Password = hasher.HashPassword(patient, model.Password);

				//if (int.TryParse(model.VaccinationCenterId.ToString(), out int vaccinationCenterId))
				//{
				//	patient.VaccinationCenterId = vaccinationCenterId;
				//}
				//else
				//{
				//	ModelState.AddModelError("VaccinationCenterId", "Invalid Vaccination Center ID");
				//	return BadRequest(ModelState);
				//}

				var EmailFalg = await _dbContext.Patients.Where(P => P.Email == model.Email).FirstOrDefaultAsync();
				if (EmailFalg == null)
				{
					await _accRepo.AddAsync(patient);
					var RPatient = await _dbContext.RegisteredPatients.FirstOrDefaultAsync(p => p.Email == model.Email);
					await _rAccRepo.DeleteAsync(RPatient);
					int Result = await _unitOfWork.Complete();
					if (Result > 0)
					{
						return Ok(new SendRegisterDTO()
						{
							Name = model.Name,
							Email = model.Email,
							Role = "Patient",
							Status = new StatuseOfResonse()
							{
								Message = true,
								Value = "Success"

							}

						});
					}
					else
					{
						return BadRequest(new StatuseOfResonse()
						{
							Message = false,
							Value = "No Rows Affected"

						});

					}
				}
				else
				{
					return BadRequest(new StatuseOfResonse()
					{
						Message = false,
						Value = "Email Already Exist"

					});
				}



			}
			else
			{
				return BadRequest(new StatuseOfResonse()
				{
					Message = false,
					Value = "Invaild Inputs in Model"

				});
			}


		}



		[Authorize(Roles = "Admin")]
		[HttpDelete("RejectUser")]
		public async Task<ActionResult<StatuseOfResonse>> RejectUser(RegisterDTO model)
		{
			if (ModelState.IsValid)
			{

				var RPatient = await _dbContext.RegisteredPatients.FirstOrDefaultAsync(p => p.Email == model.Email);
				if (RPatient == null)
				{
					return BadRequest(new StatuseOfResonse()
					{
						Message = false,
						Value = "No Waiting Users Exist"

					});
				}
				else
				{
					await _rAccRepo.DeleteAsync(RPatient);
					int Result = await _unitOfWork.Complete();
					if (Result > 0)
					{
						return Ok(new StatuseOfResonse()
						{

							Message = true,
							Value = "Rejected User Successfully"


						});
					}
					else
					{
						return BadRequest(new StatuseOfResonse()
						{
							Message = false,
							Value = "No Rows Deleted"

						});

					}
				}

			}
			else
			{
				return BadRequest(new StatuseOfResonse()
				{
					Message = false,
					Value = "Invaild Inputs in Model"

				});
			}


		}





		[HttpPost("SendRegister")]
		public async Task<ActionResult<UserDTO>> SendRegister(RegisterDTO model)
		{
			if (ModelState.IsValid)
			{
				var AdminId = _dbContext.Admins.FirstOrDefault();
				var RPatient = new RegisteredPatient
				{
					Name = model.Name,
					Email = model.Email,
					Password = model.Password,
					Role = "Patient",
					Ssn = model.Ssn,
					Phone = model.Phone,
					AdminId = AdminId.Id

				};

				var hasher = new PasswordHasher<RegisteredPatient>();
				RPatient.Password = hasher.HashPassword(RPatient, model.Password);

				//if (int.TryParse(model.VaccinationCenterId.ToString(), out int vaccinationCenterId))
				//{
				//	RPatient.VaccinationCenterId = vaccinationCenterId;
				//}
				//else
				//{
				//	ModelState.AddModelError("VaccinationCenterId", "Invalid Vaccination Center ID");
				//	return BadRequest(ModelState);
				//}
				var EmailFalg = await _dbContext.RegisteredPatients.Where(P => P.Email == model.Email).FirstOrDefaultAsync();
				if (EmailFalg == null)
				{
					await _dbContext.RegisteredPatients.AddAsync(RPatient);
					await _dbContext.SaveChangesAsync();
					return Ok(new SendRegisterDTO()
					{
						Name = model.Name,
						Email = model.Email,
						Role = "Patient",
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




		[HttpPost("Login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
		{
			if (ModelState.IsValid)
			{
				var Admin = await _dbContext.Admins.Where(A => A.Email == model.Email).FirstOrDefaultAsync();
				var Patient = await _dbContext.Patients.Where(P => P.Email == model.Email).FirstOrDefaultAsync();
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
							Value = "Wrong Email Or Password"
						});
					}

				}
				else if (Patient is not null)
				{
					var hasher = new PasswordHasher<Patient>();
					var verificationResult = hasher.VerifyHashedPassword(Patient, Patient.Password, model.Password);

					if (verificationResult == PasswordVerificationResult.Success)
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


		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<ActionResult<UserDTO>> GetAllWaitingPatients()
		{
			var rPatients = await _rAccRepo.GetAllAsync();
			if (rPatients?.Count() > 0)
			{
				var rPatientDTOs = rPatients.Select(p => new WaitingUserDTO
				{
					Id = p.Id,
					Name = p.Name,
					Email = p.Email,
					Phone = p.Phone,
					Ssn = p.Ssn,
					//VaccinationCenterName = p.VaccinationCenter?.Name,
					Status = new StatuseOfResonse
					{
						Message = true,
						Value = "Success"
					}
				}).ToList();

				return Ok(rPatientDTOs);
			}
			else
			{
				return BadRequest(new StatuseOfResonse
				{

					Message = false,
					Value = "No Waiting Users Exist"
				});
			}



		}





		[Authorize(Roles = "Admin")]
		[HttpGet("{id}")]
		public async Task<ActionResult<UserDTO>> GetWaitingPatientsById(int id)
		{
			var rPatients = await _rAccRepo.GetByIdAsync(id);
			if (rPatients is not null)
			{
				var rPatientDTOs = new WaitingUserDTO
				{
					Id = rPatients.Id,
					Name = rPatients.Name,
					Email = rPatients.Email,
					Phone = rPatients.Phone,
					Ssn = rPatients.Ssn,
					//VaccinationCenterName = p.VaccinationCenter?.Name,
					Status = new StatuseOfResonse
					{
						Message = true,
						Value = "Success"
					}
				};

				return Ok(rPatientDTOs);
			}
			else
			{
				return BadRequest(new StatuseOfResonse
				{

					Message = false,
					Value = "No Users Exist"
				});
			}



		}






		[HttpPost("AddAdmin")]
		public async Task<ActionResult<UserDTO>> AddAdmin(AdminDTO model)
		{
			if (ModelState.IsValid)
			{
				var admin = new Admin
				{
					Name = model.Name,
					Email = model.Email,
					Password = model.Password,
					Role = "Admin",
				};
				var hasher = new PasswordHasher<Admin>();
				admin.Password = hasher.HashPassword(admin, model.Password);
				await _dbContext.AddAsync(admin);
				int Result = await _dbContext.SaveChangesAsync();
				if (Result > 0)
				{
					return Ok(new UserDTO()
					{
						Name = model.Name,
						Email = model.Email,
						Role = "Admin",
						Token = await _tokenService.CreateAdminTokenAsync(admin),
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
				return BadRequest(new StatuseOfResonse
				{
					Message = false,
					Value = "Email Alrady Exist"
				});

		}

	}
}
