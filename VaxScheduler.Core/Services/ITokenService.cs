using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Identity;
using VaxScheduler.Core.Interfaces;

namespace VaxScheduler.Core.Services
{
	public interface ITokenService
	{
		Task<string> CreateAdminTokenAsync<T>(T User) where T: IUser; 
	}
}
