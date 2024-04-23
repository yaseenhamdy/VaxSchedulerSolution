using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Services;

namespace VaxScheduler.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<string> CreateAdminTokenAsync<T>(T user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user), "User cannot be null.");
			}

			List<Claim> authClaims = new List<Claim>();
			string token = string.Empty;

			if (typeof(T) == typeof(Admin))
			{
				Admin admin = user as Admin;  
				if (admin != null)
				{
					authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, admin.Name),
				new Claim(ClaimTypes.Email, admin.Email),
				new Claim(ClaimTypes.Role, admin.Role),
			};
				}
			}
			else
			{
				return "Unsupported user type for token creation.";
			}

			if (authClaims.Count > 0)
			{
				var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
				var tokenObject = new JwtSecurityToken(
					issuer: _configuration["JWT:ValidIssuer"],
					audience: _configuration["JWT:ValidAudience"],
					expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
				);

				token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
			}

			return token;
		}
	}
}
