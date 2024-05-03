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
	internal class PatientVaccinationCenterConfig : IEntityTypeConfiguration<PatientVaccinationCenter>
	{
		public void Configure(EntityTypeBuilder<PatientVaccinationCenter> builder)
		{


			builder.HasKey(vvc => new
			{
				vvc.VaccinationCenterId,
				vvc.PatientId
			});

			builder.HasOne(vvc => vvc.VaccinationCenter)
				.WithMany(p => p.PatientVaccinationCenters)
				.HasForeignKey(vvc => vvc.VaccinationCenterId);
			//.OnDelete(DeleteBehavior.Cascade);




			builder.HasOne(vvc => vvc.Patient)
				.WithMany(p => p.PatientVaccinationCenters)
				.HasForeignKey(vvc => vvc.PatientId);
			//.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
