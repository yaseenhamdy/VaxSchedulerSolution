using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaxScheduler.Core.Identity;

namespace VaxScheduler.Repository.Data.Configurations
{
	internal class AdminConfig : IEntityTypeConfiguration<Admin>
	{
		public void Configure(EntityTypeBuilder<Admin> builder)
		{
			builder.Property(v => v.Name).IsRequired();
			builder.Property(v => v.Email).IsRequired();
			builder.Property(v => v.Password).IsRequired();
			builder.Property(v => v.Role).IsRequired();
		}
	}
}
