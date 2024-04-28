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
    internal class VaccinationCenterConfig : IEntityTypeConfiguration<VaccinationCenter>
    {
        public void Configure(EntityTypeBuilder<VaccinationCenter> builder)
        {
            builder.HasOne(v => v.Admin)
                .WithMany()
                .HasForeignKey(v => v.AdminId)
	            .OnDelete(DeleteBehavior.Cascade);


			builder.Property(v=>v.Location).IsRequired();
            builder.Property(v=>v.Name).IsRequired();
            builder.Property(v => v.Email).IsRequired();
            builder.Property(v => v.Role).IsRequired();
			builder.Property(v => v.Password).IsRequired();
			






		}



	}
}
