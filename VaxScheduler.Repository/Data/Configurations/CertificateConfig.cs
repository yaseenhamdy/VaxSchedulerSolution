using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;

namespace VaxScheduler.Repository.Data.Configurations
{
	internal class CertificateConfig : IEntityTypeConfiguration<Certificate>
	{
		public void Configure(EntityTypeBuilder<Certificate> builder)
		{
			builder.Property(v => v.Name).IsRequired();



			builder.HasOne(c => c.Patient)
				.WithMany()
				.HasForeignKey(c => c.PatientId)
					.OnDelete(DeleteBehavior.NoAction);

			//.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(c => c.VaccinationCenter)
			 .WithMany()
			 .HasForeignKey(p => p.VaccinationCenterId)
				 .OnDelete(DeleteBehavior.NoAction);

			//.OnDelete(DeleteBehavior.Cascade);


		}


	}
}
