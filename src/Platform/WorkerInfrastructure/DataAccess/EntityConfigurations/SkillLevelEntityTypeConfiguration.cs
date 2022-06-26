using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerInfrastructure.DataAccess.EntityConfigurations
{
    public class SkillLevelEntityTypeConfiguration : IEntityTypeConfiguration<SkillLevel>
    {
        public void Configure(EntityTypeBuilder<SkillLevel> configuration)
        {
            configuration.ToTable("SkillLevels");
            configuration.HasKey(ct => ct.Id);
            configuration.Property(ct => ct.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            configuration.Property(ct => ct.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}