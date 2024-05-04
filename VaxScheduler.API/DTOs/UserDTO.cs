using VaxScheduler.Core.Errors;

namespace VaxScheduler.API.DTOs
{
	public class UserDTO
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
		public string Role { get; set; }
		public StatuseOfResonse Status { get; set; }

	}
}
