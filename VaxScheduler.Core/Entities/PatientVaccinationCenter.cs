using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxScheduler.Core.Entities
{
	public class PatientVaccinationCenter
	{
		public Patient Patient { get; set; }
		public int PatientId { get; set; }

		public VaccinationCenter VaccinationCenter { get; set; }
		public int VaccinationCenterId { get; set; }
	}
}
