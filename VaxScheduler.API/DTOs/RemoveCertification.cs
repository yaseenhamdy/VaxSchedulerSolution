namespace VaxScheduler.API.DTOs
{
	public class RemoveCertification
	{
		public int PatientId { get; set; }
		public int VaccineId { get; set; }
		public int VaccinationCenterId { get; set; }
	}
}
