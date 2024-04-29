using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Security.Claims;
using System.Text;
using VaxScheduler.Core.Repositories;
using VaxScheduler.Core.Services;
using VaxScheduler.Repository;
using VaxScheduler.Repository.Data;
using VaxScheduler.Services;

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
				Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
					//b => b.MigrationsAssembly("VaxScheduler.API")

					);
			});
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin", builder =>
				{
					builder
						.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader();
				});
			});
			builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

			//builder.Services.AddControllers().AddJsonOptions(options =>
			//{
			//	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
			//});




			#region For Authentication
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
					.AddJwtBearer(options =>
			   {
				   options.RequireHttpsMetadata = false; options.SaveToken = true;
				   options.TokenValidationParameters = new TokenValidationParameters
				   {
					   ValidateIssuerSigningKey = true,
					   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
					   ValidateIssuer = true,
					   ValidateAudience = true,
					   ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
					   ValidAudience = builder.Configuration["JWT:ValidAudience"],
					   RoleClaimType = ClaimTypes.Role,  
					   ClockSkew = TimeSpan.Zero
				   };
			   });
			#endregion


			builder.Services.AddScoped<ITokenService, TokenService>();

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

			app.UseAuthentication(); // This must be before UseAuthorization

			app.UseAuthorization();


			app.MapControllers();

			app.UseCors("AllowSpecificOrigin");

			app.Run();
		}
	}
}
