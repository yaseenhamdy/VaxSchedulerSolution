using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Core.Services;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.API.Controllers
{
	public class PatientsController : APIBaseController
	{
		private readonly VaxDbContext _dbContext;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Patient> _patientRepo;

		public PatientsController(VaxDbContext dbContext, IUnitOfWork unitOfWork, IGenericRepository<Patient> patientRepo)
		{
			_dbContext = dbContext;
			_unitOfWork = unitOfWork;
			_patientRepo = patientRepo;
		}


		[HttpGet("some-action")]
		public IActionResult SomeAction()
		{
			var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
			if (userRole != "Center")
			{
				return Unauthorized("You do not have permission to view this resource.");
			}
			return Ok("Access granted to protected resource.");
		}



		[HttpPost("SendDose")]
		public async Task<ActionResult<StatuseOfResonse>> SendDose(SendDoseDTo model)
		{
			var patientVaccines = _dbContext.patientVaccines
								   .Where(PV => PV.PatientId == model.PatientId && PV.VaccineId == model.VaccineId);

			if (!await patientVaccines.AnyAsync())
			{
				var newPV = new PatientVaccine
				{
					PatientId = model.PatientId,
					VaccineId = model.VaccineId,
					FirstDose = null,
					SecondDose = null,
					FlagFirstDose = 0,
					FlagShow =1,
					FlagSecondDose = null,
					VaccinationCenterId = model.VaccinationCenterId
				};

				await _dbContext.patientVaccines.AddAsync(newPV);
				int result = await _unitOfWork.Complete();

				if (result > 0)
				{
					return Ok(new StatuseOfResonse
					{
						Message = true,
						Value = "Request to Take Dose Successfully Sent"
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
				var patientVaccine = await patientVaccines.FirstOrDefaultAsync();

				if (patientVaccine.FirstDose == 1 && patientVaccine.SecondDose == null)
				{
					patientVaccine.FlagSecondDose = 0;
					patientVaccine.FirstDose = 1;
					patientVaccine.FlagShow = 1;
					patientVaccine.FlagFirstDose = 1;
					_dbContext.patientVaccines.Update(patientVaccine);
					int result = await _unitOfWork.Complete();

					if (result > 0)
					{
						return Ok(new StatuseOfResonse
						{
							Message = true,
							Value = "Second Dose Flag Updated Successfully"
						});
					}
					else
					{
						return BadRequest(new StatuseOfResonse
						{
							Message = false,
							Value = "No Changes Made"
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

		}





		[HttpGet("{id}")]
		public async Task<ActionResult<List<GetPatientDTO>>> GetPatientById(int id)
		{

			var patient = await _patientRepo.GetByIdAsync(id);
			if (patient == null)
			{
				return NotFound("Patient not found.");
			}

			var patientDto = new GetPatientDTO
			{
				Id = patient.Id,
				Name = patient.Name,
				Email = patient.Email,
				Phone = patient.Phone,
				Ssn = patient.Ssn
			};

			HashSet<int> uniqueVaccinationCenters = new HashSet<int>();

			foreach (var vaccineRecord in patient.patientVaccines)
			{
				var vaccineInfo = new VaccineInfoDTO
				{
					VaccineId = vaccineRecord.VaccineId,
					VaccineName = vaccineRecord.Vaccine?.Name ?? "Unknown",
					VaccinationCenterId = vaccineRecord.VaccinationCenterId,
					VaccinationCenterName = vaccineRecord.VaccinationCenter?.Name ?? "Unknown",
					FirstDose = vaccineRecord.FirstDose.HasValue && vaccineRecord.FirstDose.Value == 1,
					SecondDose = vaccineRecord.SecondDose.HasValue && vaccineRecord.SecondDose.Value == 1
				};

				patientDto.Vaccines.Add(vaccineInfo);

				// Add to vaccination center list if not already included
				//if (!uniqueVaccinationCenters.Contains(vaccineRecord.VaccinationCenterId))
				//{
				//	uniqueVaccinationCenters.Add(vaccineRecord.VaccinationCenterId);
				//	patientDto.VaccinationCenters.Add(new VaccinationCenterInfoDTO
				//	{
				//		VaccinationCenterId = vaccineRecord.VaccinationCenterId,
				//		VaccinationCenterName = vaccineRecord.VaccinationCenter?.Name ?? "Unknown"
				//	});
				//}
			}

			return Ok(patientDto);

		}




		[HttpGet("GetAllPatients/{vaccinationCenterId}")]
		[Authorize(Roles = "Center")]

		public async Task<ActionResult<List<GetAllPatientsDTO>>> GetAllPatients(int vaccinationCenterId)
		{



			var patients = await _dbContext.patientVaccines
				  .Where(pv => (pv.FirstDose == 1 || pv.SecondDose == 1) && pv.VaccinationCenterId == vaccinationCenterId)
				  .Select(pv => new GetAllPatientsDTO
				  {
					  Id = pv.PatientId,
					  Name = pv.Patient.Name,
					  Email = pv.Patient.Email,
					  Ssn = pv.Patient.Ssn,
					  Phone = pv.Patient.Phone
				  })
				  .Distinct()
				  .ToListAsync();

			if (!patients.Any())
			{
				return NotFound(new StatuseOfResonse
				{
					Message = false,
					Value = "No patients found who have received the first or second dose at the specified vaccination center"
				});
			}

			return Ok(patients);

		}
	}
}
