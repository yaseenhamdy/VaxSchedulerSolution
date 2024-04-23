using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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


		}
	}
}
