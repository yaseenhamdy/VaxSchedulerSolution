using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxScheduler.Core.Entities
{
    public class PatientVaccine
    {
        public Patient Patient { get; set; }
        public int PatientId { get; set; }

        public int? FirstDose { get; set; }
		public int? SecondDose { get; set; }
        public int? FlagFirstDose { get; set; }
        public int? FlagSecondDose { get; set; }

        public Vaccine Vaccine { get; set; }
        public int VaccineId { get;set; }


		public VaccinationCenter VaccinationCenter { get; set; }
		public int VaccinationCenterId { get; set; }
	}
}
