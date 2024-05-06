namespace VaxScheduler.API.DTOs
{
	public class CertificateDetailsDTO
	{
		public int PatientId { get; set; }
		public string PatientName { get; set; }
		public string PatientEmail { get; set; }
		public int VaccineId { get; set; }
		public string VaccineName { get; set; }
	}
}
