using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Repository.Data
{
	public class VaxDbContext : DbContext
	{
		public VaxDbContext(DbContextOptions<VaxDbContext> Options) : base(Options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}
		public DbSet<Admin> Admins { get; set; }
		public DbSet<VaccinationCenter> VaccinationCenters { get; set; }
		public DbSet<Vaccine> Vaccines { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Certificate> Certificates { get; set; }

		public DbSet<PatientVaccine> patientVaccines { get; set; }

		public DbSet<VaccineVaccinationCenter> vaccineVaccinationCenters { get; set; }
	}
}
