using AppShareServices.Events;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess
{
    public class EventContext : DbContext
    {
        public DbSet<DomainEventRecord> DomainEventRecords { get; set; }

        public EventContext()
        {
        }

        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DomainEventEntityTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=EventDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }
    }
}
