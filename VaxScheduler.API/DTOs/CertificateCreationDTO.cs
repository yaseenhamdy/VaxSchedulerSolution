namespace VaxScheduler.API.DTOs
{
	public class CertificateCreationDTO
	{
		public int PatientId { get; set; }
		public int VaccineId { get; set; }
        public int VaccinationCenterId { get; set; }
        public string ImageUrl { get; set; }
	}
}
