using AppShareServices.DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using SkillDomain.AgreegateModels.SkillAgreegate;
using Action = SkillDomain.AgreegateModels.SkillAgreegate.Action;

namespace SkillInfrastructure.DataAccess
{
    public class SkillContext : DbContext, IDatabaseService
    {
        public const string DEFAULT_SCHEMA = "workerdb";

        public DbSet<Skill> Skills { get; set; }
        public DbSet<WorkerSkillLevel> WorkerSkillLevels { get; set; }
        public DbSet<SpecificationSkillLevel> SpecificationSkillLevels { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<MatrixSkill> MatrixSkills { get; set; }

        DbSet<T> IDatabaseService.GetDbSet<T>()
        {
            return Set<T>();
        }

        Task IDatabaseService.SaveChanges()
        {
            return Task.FromResult(base.SaveChanges());
        }
        public SkillContext()
        {
        }
        public SkillContext(DbContextOptions<SkillContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Move to out side
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=SkillDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        /// <summary>
        /// ref: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // WorkerSkillLevel
            modelBuilder.Entity<WorkerSkillLevel>().ToTable("WorkerSkillLevels").HasKey(c => c.Id);
            modelBuilder.Entity<WorkerSkillLevel>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<WorkerSkillLevel>().Property(c => c.Name)
                 .HasMaxLength(250)
                 .IsRequired();

            // SpecificationSkillLevel
            modelBuilder.Entity<SpecificationSkillLevel>().ToTable("SpecificationSkillLevels").HasKey(c => c.Id);
            modelBuilder.Entity<SpecificationSkillLevel>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<SpecificationSkillLevel>().Property(c => c.Name)
                 .HasMaxLength(250)
                 .IsRequired();

            // MatrixSkill
            modelBuilder.Entity<MatrixSkill>()
            .HasOne(s => s.Skill)
            .WithMany(c => c.MatrixSkills)
            .HasForeignKey(s => s.SkillId);
        }
    }
}
