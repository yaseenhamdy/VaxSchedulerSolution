namespace VaxScheduler.API.DTOs
{
	public class GetAllVaccineDTO
	{
        public int Id { get; set; }
        public string Name { get; set; }

		public int DurationBetweenDoses { get; set; }

		public string Precautions { get; set; }
		public List<VaccineCenterVaccineInVaccineDTO> VaccinationCenterName { get; set; }
	}
}
