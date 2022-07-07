using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace SpecificationInfrastructure.DataAccess;

public class SpecificationContext : DbContext, IDatabaseService
{
    public SpecificationContext() { }

    DbSet<T> IDatabaseService.GetDbSet<T>()
    {
        return Set<T>();
    }

    Task IDatabaseService.SaveChanges()
    {
        return Task.FromResult(base.SaveChanges());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // TODO: Move to out side
            optionsBuilder.UseMySql("Server=localhost;port=3306;Database=SpecificationDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
        }
    }

    /// <summary>
    /// ref: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SettingType>().ToTable("SettingTypes").HasKey(c => c.Id);
        modelBuilder.Entity<SettingType>().Property(c => c.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        modelBuilder.Entity<Setting>().HasOne(c => c.SettingType);

        modelBuilder.Entity<Setting>()
        .HasMany(i => i.Specifications)
        .WithMany(i => i.Settings)
        .UsingEntity<SpecificationSetting>(
            j => j
                .HasOne(w => w.Specification)
                .WithMany(w => w.SpecificationSettings)
                .HasForeignKey(w => w.SpecificationId)
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne(w => w.Setting)
                .WithMany(w => w.SpecificationSettings)
                .HasForeignKey(w => w.SettingId)
                .OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.Ignore(w => w.Id).HasKey(w => new { w.SettingId, w.SpecificationId });
            });
    }

}

