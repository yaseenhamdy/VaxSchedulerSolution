using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Repository.Data
{
	public class VaxDbContextSeed
	{
		async public static Task SeedAsync(VaxDbContext dbContext)
		{
			//Seeding Admin
			if (!dbContext.Admins.Any())
			{
				var AdminFilePath = File.ReadAllText("../VaxScheduler.Repository/Data/DataSeed/admin.json");
				var admins = JsonSerializer.Deserialize<List<Admin>>(AdminFilePath);
				if (admins?.Count > 0)
				{
					foreach (var admin in admins)
						await dbContext.Admins.AddAsync(admin);
				}
				await dbContext.SaveChangesAsync();
			}


			if (!dbContext.VaccinationCenters.Any())
			{
				var CenterFilePath = File.ReadAllText("../VaxScheduler.Repository/Data/DataSeed/vaccinationcenter.json");
				var centers = JsonSerializer.Deserialize<List<VaccinationCenter>>(CenterFilePath);
				if (centers?.Count > 0)
				{
					foreach (var center in centers)
						await dbContext.VaccinationCenters.AddAsync(center);
				}
				await dbContext.SaveChangesAsync();
			}


			if (!dbContext.Patients.Any())
			{
				var PatientFilePath = File.ReadAllText("../VaxScheduler.Repository/Data/DataSeed/patient.json");
				var patients = JsonSerializer.Deserialize<List<Patient>>(PatientFilePath);
				if (patients?.Count > 0)
				{
					foreach (var patient in patients)
						await dbContext.Patients.AddAsync(patient);
				}
				await dbContext.SaveChangesAsync();
			}


		}
	}
}
