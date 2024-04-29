using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Core.Entities
{
	public class RegisteredPatient : Patient
	{
		public VaccinationCenter VaccinationCenter { get; set; }

		public ICollection<PatientVaccine> patientVaccines { get; set; }
	}
}
