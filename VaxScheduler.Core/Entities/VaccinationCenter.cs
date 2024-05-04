using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Interfaces;

namespace VaxScheduler.Core.Entities
{
    public class VaccinationCenter : BaseEntity,IUser
    { 
      
        public string Name { get; set; }

        [EmailAddress]
		public string Email { get; set; }


		[RegularExpression("(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$"
			, ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter" +
			", one lowercase letter, and one number or special character.")]
		public string Password { get; set; }

		public string Location { get; set; }
		public string Role { get; set; }

		public Admin Admin { get; set; }
        public int AdminId { get; set; }
		public ICollection<VaccineVaccinationCenter> VaccineVaccinationCenter { get; set; }
		public ICollection<PatientVaccinationCenter> PatientVaccinationCenters { get; set; }
		public ICollection<PatientVaccine> patientVaccines { get; set; }



	}
}
