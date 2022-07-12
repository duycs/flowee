using AppShareServices.DataAccess.Persistences;
using JobDomain.AgreegateModels.JobAgreegate;
using Microsoft.EntityFrameworkCore;

namespace JobInfrastructure.DataAccess
{
    public class JobContext : DbContext, IDatabaseService
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobStatus> JobStatus { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<StepStatus> StepStatus { get; set; }
        public DbSet<StructType> StructTypes { get; set; }

        /// <summary>
        /// Must concreate first for ef migrations
        /// </summary>
        public JobContext() { }

        public JobContext(DbContextOptions<JobContext> options) : base(options) { }

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
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=JobDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        /// <summary>
        /// ref: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobStatus>().ToTable("JobStatus").HasKey(c => c.Id);
            modelBuilder.Entity<JobStatus>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<JobStatus>().Property(c => c.Name)
               .HasMaxLength(250)
               .IsRequired();

            modelBuilder.Entity<StructType>().ToTable("StructTypes").HasKey(c => c.Id);
            modelBuilder.Entity<StructType>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<StructType>().Property(c => c.Name)
               .HasMaxLength(250)
               .IsRequired();

            modelBuilder.Entity<StepStatus>().ToTable("StepStatus").HasKey(c => c.Id);
            modelBuilder.Entity<StepStatus>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<StepStatus>().Property(c => c.Name)
               .HasMaxLength(250)
               .IsRequired();

            modelBuilder.Entity<Job>().HasOne(c => c.JobStatus);
            modelBuilder.Entity<Job>().HasMany(c => c.Operations).WithOne(c => c.Job);

            modelBuilder.Entity<Step>().HasOne(c => c.StepStatus);
        }
    }
}
