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
    internal class PatientVaccineConfig : IEntityTypeConfiguration<PatientVaccine>
    {
        public void Configure(EntityTypeBuilder<PatientVaccine> builder)
        {
            builder.HasKey(pv => new
            {
                pv.PatientId,
                pv.VaccineId
            });

            builder.HasOne(pv => pv.Patient)
                .WithMany(p => p.patientVaccines)
                .HasForeignKey(pv => pv.PatientId)
					.OnDelete(DeleteBehavior.NoAction);

			//.OnDelete(DeleteBehavior.Cascade);




			builder.HasOne(pv => pv.Vaccine)
                .WithMany(p => p.patientVaccines)
                .HasForeignKey(pv => pv.VaccineId);
               




        }
    }
}
