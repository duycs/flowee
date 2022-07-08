using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using JobDomain.AgreegateModels.JobAgreegate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobInfrastructure.DataAccess
{
    public class JobContext : DbContext, IDatabaseService
    {
        public DbSet<JobStatus> JobStatus { get; set; }
        public DbSet<JobStepStatus> JobStepStatus { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<JobStep> JobSteps { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public JobContext() { }

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

            modelBuilder.Entity<JobType>().ToTable("JobTypes").HasKey(c => c.Id);
            modelBuilder.Entity<JobType>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            modelBuilder.Entity<Job>().HasMany(c => c.JobSteps).WithOne(c => c.Job);
        }
    }
}
