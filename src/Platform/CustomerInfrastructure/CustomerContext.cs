using AppShareServices.DataAccess.Persistences;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using Microsoft.EntityFrameworkCore;

namespace CustomerInfrastructure
{
    public class CustomerContext : DbContext, IDatabaseService
    {
        public const string DEFAULT_SCHEMA = "customerdb";

        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PriorityLevel> PriorityLevels { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }


        DbSet<T> IDatabaseService.GetDbSet<T>()
        {
            return Set<T>();
        }

        Task IDatabaseService.SaveChanges()
        {
            return Task.FromResult(base.SaveChanges());
        }

        public CustomerContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Move to out side
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=CustomerDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        /// <summary>
        /// ref: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PriorityLevel
            modelBuilder.Entity<PriorityLevel>().ToTable("PriorityLevels").HasKey(c => c.Id);
            modelBuilder.Entity<PriorityLevel>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<PriorityLevel>().Property(c => c.Name)
               .HasMaxLength(250)
               .IsRequired();

            // Customer - Currency,PriorityLevel,PaymentMethods
            modelBuilder.Entity<Customer>().HasOne<PriorityLevel>();
            modelBuilder.Entity<Customer>().HasOne<Currency>();
            modelBuilder.Entity<Customer>().HasMany(b => b.PaymentMethods)
               .WithOne()
               //.HasForeignKey("CustomerId")
               .OnDelete(DeleteBehavior.Cascade);
        }

    }
}