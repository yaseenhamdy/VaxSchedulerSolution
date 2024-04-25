namespace VaxScheduler.API.DTOs
{
	public class AddVaccineDTO
	{
		public string Name { get; set; }

		public int DurationBetweenDoses { get; set; }

		public string Precautions { get; set; }
	}
}
