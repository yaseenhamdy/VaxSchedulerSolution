using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace VaxScheduler.API.DTOs

{
	public class GetPatientDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Ssn { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		//public bool FirstDose { get; set; }
		//public bool SecondDose { get; set; }
		public List<VaccineInfoDTO> Vaccines { get; set; }
		//public List<string> VaccinationCenterNames { get; set; }

		public GetPatientDTO()
		{
			Vaccines = new List<VaccineInfoDTO>();
			//VaccinationCenterNames = new List<string>();
		}
	}
}
