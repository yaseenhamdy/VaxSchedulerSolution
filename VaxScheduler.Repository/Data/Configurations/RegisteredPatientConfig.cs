﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Entities;

namespace VaxScheduler.Repository.Data.Configurations
{
	internal class RegisteredPatientConfig : IEntityTypeConfiguration<RegisteredPatient>
	{
		public void Configure(EntityTypeBuilder<RegisteredPatient> builder)
		{
			builder.Property(v => v.Name).IsRequired();
			builder.Property(v => v.Email).IsRequired();
			builder.Property(v => v.Password).IsRequired();
			builder.Property(v => v.Phone).IsRequired();
			builder.Property(v => v.Role).IsRequired();


			builder.HasOne(p => p.Admin)
			   .WithMany()
			   .HasForeignKey(p => p.AdminId)
				   .OnDelete(DeleteBehavior.NoAction);

			//.OnDelete(DeleteBehavior.Cascade);



			builder.HasOne(p => p.VaccinationCenter)
				.WithMany()
				.HasForeignKey(p => p.VaccinationCenterId);





		}
	}

}
