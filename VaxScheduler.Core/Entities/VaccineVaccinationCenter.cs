using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxScheduler.Core.Entities
{
    public class VaccineVaccinationCenter
    {

        public VaccinationCenter VaccinationCenter { get; set; }
        public int VaccinationCenterId { get; set; }

        public Vaccine Vaccine { get; set; }
        public int VaccineId { get; set; }
    }
}
