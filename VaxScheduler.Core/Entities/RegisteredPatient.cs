using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Core.Entities
{
	public class RegisteredPatient :BaseEntity
	{
		public string Name { get; set; }
		public string Ssn { get; set; }
		public string Role { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		[RegularExpression("(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$"
			, ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter" +
			", one lowercase letter, and one number or special character.")]
		public string Password { get; set; }

		[Phone]
		public string Phone { get; set; }

		public Admin Admin { get; set; }
		public int AdminId { get; set; }

		public int VaccinationCenterId { get; set; }
		public VaccinationCenter VaccinationCenter { get; set; }

		public ICollection<PatientVaccine> patientVaccines { get; set; }
	}
}
