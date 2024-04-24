using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaxScheduler.Core.Interfaces
{
	public interface IUser
	{
		string Name { get; }
		string Email { get; }
		string Role { get; }
	}
}
