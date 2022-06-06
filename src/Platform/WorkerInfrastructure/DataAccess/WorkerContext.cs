using AppShareServices.DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.TimeKeepingAgreegate;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerInfrastructure.DataAccess
{
    public class WorkerContext : DbContext, IDatabaseService
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<TimeKeeping> TimeKeepings { get; set; }

        public WorkerContext(DbContextOptions<WorkerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        Task IDatabaseService.SaveChanges()
        {
            return Task.FromResult(base.SaveChanges());
        }
    }
}
