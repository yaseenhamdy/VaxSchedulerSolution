using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using VaxScheduler.Repository.Data;

namespace VaxScheduler.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<VaxDbContext>(Options =>
			{
				Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			var app = builder.Build();

			#region Update Database & Seeding Data
			using var scope = app.Services.CreateScope();

			var service = scope.ServiceProvider;
			var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
			try
			{
				var dbContext = service.GetRequiredService<VaxDbContext>();

				await dbContext.Database.MigrateAsync();

				// Seeding Data
				await VaxDbContextSeed.SeedAsync(dbContext);
			}
			catch (Exception ex)
			{
				var logger = LoggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error Occured");
			}

			#endregion

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
