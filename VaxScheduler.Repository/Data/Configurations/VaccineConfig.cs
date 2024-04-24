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
    internal class VaccineConfig : IEntityTypeConfiguration<Vaccine>
    {
        public void Configure(EntityTypeBuilder<Vaccine> builder)
        {
            builder.HasOne(A => A.Admin)
                 .WithMany()
                 .HasForeignKey(A => A.AdminId)
				 .OnDelete(DeleteBehavior.Restrict);


            builder.Property(v=>v.Name).IsRequired();


            builder.Property(v => v.Precautions).IsRequired();
        }
    }
}
