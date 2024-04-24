using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VaxScheduler.Core.Identity
{
	public class Admin
	{
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress]
		public string Email { get; set; }

		[RegularExpression("(?=^.{8,}$)((?=.*\\d)|(?=.*\\W+))(?![.\\n])(?=.*[A-Z])(?=.*[a-z]).*$"
            , ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter" +
            ", one lowercase letter, and one number or special character.")]
		public string Password { get; set; }
		public string  Role { get; set; }
    }
}
