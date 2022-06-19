using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.TimeKeepingAgreegate;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerInfrastructure.DataAccess.EntityConfigurations
{
    public class SkillLevelEntityTypeConfiguration : IEntityTypeConfiguration<SkillLevel>
    {
        public void Configure(EntityTypeBuilder<SkillLevel> configuration)
        {
            configuration.ToTable("skilllevels");

            configuration.HasKey(ct => ct.Id);

            configuration.Property(ct => ct.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            configuration.Property(ct => ct.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}