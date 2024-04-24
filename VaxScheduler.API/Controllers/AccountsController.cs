using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using VaxScheduler.API.DTOs;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Services;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.API.Controllers
{
	public class AccountsController : APIBaseController
	{
		private readonly VaxDbContext _dbContext;
		private readonly ITokenService _tokenService;

		public AccountsController(VaxDbContext dbContext , ITokenService tokenService)
		{
			_dbContext = dbContext;
			_tokenService = tokenService;
		}

		//[HttpPost("Register")]
		//public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//	}

		//}

		[HttpPost("Login")]
		public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
		{
			if (ModelState.IsValid)
			{
				var Admin = await _dbContext.Admins.Where(A => A.Email == model.Email && A.Password == model.Password).FirstOrDefaultAsync();
				if (Admin is not null)
				{
					return Ok(new UserDTO()
					{
						Name = Admin.Name,
						Token = await _tokenService.CreateAdminTokenAsync(Admin),
						Email = Admin.Email,
						Role = Admin.Role,
					});
				}
				else
				{
					return BadRequest();
				}
			}
			else { return BadRequest(); }

		}
	}
}
