using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class GetAllWaitingDosesDTO
	{
		public int PatientId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int VaccineId { get; set; }
		public string VaccineName { get; set; }

	}
}
