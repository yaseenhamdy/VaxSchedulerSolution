using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class AddVaccineResponse
	{
		public string Name { get; set; }
		public StatuseOfResonse Status { get; set; }
	}
}
