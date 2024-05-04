using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Interfaces;
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

		public async Task<string> CreateAdminTokenAsync<T>(T user) where  T :IUser
		{

			if (user == null)
			{
				throw new ArgumentNullException(nameof(user), "User cannot be null.");
			}

			List<Claim> authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.Name),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role),
		};

			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
			var tokenObject = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
			);

			return new JwtSecurityTokenHandler().WriteToken(tokenObject);
		}
	}
}
