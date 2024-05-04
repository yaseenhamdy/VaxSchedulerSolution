using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class GetAllPatientsDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Ssn { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }

	}
}
