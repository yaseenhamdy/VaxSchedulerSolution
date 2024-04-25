using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Errors;
using VaxScheduler.Core.Repositories;

namespace VaxScheduler.API.Controllers
{
   
    public class VaccineController : APIBaseController
    {
        private readonly IGenericRepository<Vaccine> _vaccineRepo;

        public VaccineController(IGenericRepository<Vaccine> vaccineRepo)
        {
            _vaccineRepo = vaccineRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vaccine>>> GetAllVaccine()
        {
            var Vaccines =await _vaccineRepo.GetAllAsync(); 

          var vaccineDTOs = Vaccines.Select(v=>new VaccineDTO
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
    }
}
