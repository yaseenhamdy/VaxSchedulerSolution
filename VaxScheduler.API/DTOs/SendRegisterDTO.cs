using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class SendRegisterDTO
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public StatuseOfResonse Status { get; set; }
	}
}
