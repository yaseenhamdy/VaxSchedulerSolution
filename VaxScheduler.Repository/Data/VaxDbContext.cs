using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Repository.Data
{
	public class VaxDbContext : DbContext
	{
		public VaxDbContext(DbContextOptions<VaxDbContext> Options) : base(Options)
		{

		}
		public DbSet<Admin> Admins { get; set; }
	}
}
