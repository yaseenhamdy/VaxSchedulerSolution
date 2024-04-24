using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;


namespace VaxScheduler.Core.Entities
{
    public class Vaccine : BaseEntity
    {
       
        public string Name { get; set; }

        public int DurationBetweenDoses { get; set; }

        public string Precautions { get; set; }

        public Admin Admin { get; set; }
        public int AdminId { get; set; }


        public ICollection<PatientVaccine> patientVaccines { get; set; }

        public ICollection<VaccineVaccinationCenter> VaccineVaccinationCenter { get; set; }

    }
}
