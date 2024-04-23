using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Core.Services
{
	public interface ITokenService
	{
		Task<string> CreateAdminTokenAsync<T>(T User); 
	}
}
