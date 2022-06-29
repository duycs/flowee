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
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            configuration.Property(c => c.Name)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}