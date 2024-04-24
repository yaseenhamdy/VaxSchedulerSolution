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
    internal class VaccineVaccinationCenterConfig : IEntityTypeConfiguration<VaccineVaccinationCenter>
    {
        public void Configure(EntityTypeBuilder<VaccineVaccinationCenter> builder)
        {
            builder.HasKey(vvc => new
            {
                vvc.VaccinationCenterId,
                vvc.VaccineId
            });

            builder.HasOne(vvc => vvc.Vaccine)
                .WithMany(p => p.VaccineVaccinationCenter)
                .HasForeignKey(vvc => vvc.VaccineId)
				.OnDelete(DeleteBehavior.Restrict); ;




            builder.HasOne(vvc => vvc.VaccinationCenter)
                .WithMany(p => p.VaccineVaccinationCenter)
                .HasForeignKey(vvc => vvc.VaccinationCenterId)
				.OnDelete(DeleteBehavior.Restrict); 
               

        }
    }
}
