using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace SpecificationInfrastructure.DataAccess;

public class SpecificationContext : DbContext, IDatabaseService
{
    public DbSet<Condition> Conditions { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<RuleSetting> RuleSettings { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<SettingType> SettingTypes { get; set; }
    public DbSet<Specification> Specifications { get; set; }
    public DbSet<SpecificationRule> SpecificationRules { get; set; }

    public SpecificationContext() { }

    public SpecificationContext(DbContextOptions<SpecificationContext> options) : base(options)
    {
    }

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

        modelBuilder.Entity<Condition>().ToTable("Conditions").HasKey(c => c.Id);
        modelBuilder.Entity<Condition>().Property(c => c.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();
        modelBuilder.Entity<Condition>().Property(c => c.Name)
             .HasMaxLength(250)
             .IsRequired();

        modelBuilder.Entity<Operator>().ToTable("Operators").HasKey(c => c.Id);
        modelBuilder.Entity<Operator>().Property(c => c.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();
        modelBuilder.Entity<Operator>().Property(c => c.Name)
             .HasMaxLength(250)
             .IsRequired();

        modelBuilder.Entity<SettingType>().ToTable("SettingTypes").HasKey(c => c.Id);
        modelBuilder.Entity<SettingType>().Property(c => c.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();
        modelBuilder.Entity<SettingType>().Property(c => c.Name)
             .HasMaxLength(250)
             .IsRequired();

        // Setting
        modelBuilder.Entity<Setting>().HasOne(c => c.SettingType);

        // Rule
        modelBuilder.Entity<Rule>().HasOne(c => c.Condition);
        modelBuilder.Entity<Rule>().HasOne(c => c.Setting);
        modelBuilder.Entity<Rule>().HasOne(c => c.Operator);

        // Settings - Rules
        modelBuilder.Entity<Setting>()
        .HasMany(i => i.Rules)
        .WithMany(i => i.Settings)
        .UsingEntity<RuleSetting>(
            j => j
                .HasOne(w => w.Rule)
                .WithMany(w => w.RuleSettings)
                .HasForeignKey(w => w.RuleId)
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne(w => w.Setting)
                .WithMany(w => w.RuleSettings)
                .HasForeignKey(w => w.SettingId)
                .OnDelete(DeleteBehavior.Cascade),
            j =>
            {
                j.Ignore(w => w.Id).HasKey(w => new { w.RuleId, w.SettingId });
            });

        // Specifications - Rules
        modelBuilder.Entity<Specification>()
       .HasMany(i => i.Rules)
       .WithMany(i => i.Specifications)
       .UsingEntity<SpecificationRule>(
           j => j
               .HasOne(w => w.Rule)
               .WithMany(w => w.SpecificationRules)
               .HasForeignKey(w => w.RuleId)
               .OnDelete(DeleteBehavior.Cascade),
           j => j
               .HasOne(w => w.Specification)
               .WithMany(w => w.SpecificationRules)
               .HasForeignKey(w => w.SpecificationId)
               .OnDelete(DeleteBehavior.Cascade),
           j =>
           {
               j.Ignore(w => w.Id).HasKey(w => new { w.SpecificationId, w.RuleId });
           });
    }

}

