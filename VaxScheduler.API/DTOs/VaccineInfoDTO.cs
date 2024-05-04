namespace VaxScheduler.API.DTOs
{
	public class VaccineInfoDTO
	{
		public int VaccineId { get; set; }
		public string VaccineName { get; set; }
		public int VaccinationCenterId { get; set; }
		public string VaccinationCenterName { get; set; }
		public bool FirstDose { get; set; }
		public bool SecondDose { get; set; }
	}
}
