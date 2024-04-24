using System.ComponentModel.DataAnnotations;

namespace VaxScheduler.API.DTOs
{
	public class VaccinationCenterDTO
	{
        [Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[RegularExpression("(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$"
			, ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter" +
			", one lowercase letter, and one number or special character.")]
		public string Password { get; set; }

		[Required]
		public string Location { get; set; }

    }
}
