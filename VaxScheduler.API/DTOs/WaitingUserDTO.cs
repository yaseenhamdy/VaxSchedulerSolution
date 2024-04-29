using System.ComponentModel.DataAnnotations;
using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class WaitingUserDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Ssn { get; set; }
		public string Email { get; set; }

		public string Phone { get; set; }

		public string VaccinationCenterName { get; set; }
		public StatuseOfResonse Status { get; set; }
	}
}
