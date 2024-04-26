using VaxScheduler.Core.Entities;

namespace VaxScheduler.API.DTOs
{
	public class GetAllCenter
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location  { get; set; }
		public string Role { get; set; }
		public List<VaccineVaccinationCenterDto> VaccineNames { get; set; }

    }
}
