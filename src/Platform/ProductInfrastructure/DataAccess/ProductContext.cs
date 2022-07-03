using System;
using AppShareServices.DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;
using ProductDomain.AgreegateModels.ProductAgreegate;

namespace ProductInfrastructure.DataAccess
{
    public class ProductContext : DbContext, IDatabaseService
    {
        public DbSet<Addon> Addons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductLevel> ProductLevels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAddon> ProductAddons { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        DbSet<T> IDatabaseService.GetDbSet<T>()
        {
            return Set<T>();
        }

        Task IDatabaseService.SaveChanges()
        {
            return Task.FromResult(base.SaveChanges());
        }

        public ProductContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Move to out side
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=ProductDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        /// <summary>
        /// ref: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ProductLevel
            modelBuilder.Entity<ProductLevel>().ToTable("ProductLevels").HasKey(c => c.Id);
            modelBuilder.Entity<ProductLevel>().Property(c => c.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<ProductLevel>().Property(c => c.Name)
               .HasMaxLength(250)
               .IsRequired();

            // Product-ProductLevel
            modelBuilder.Entity<Product>().HasOne<ProductLevel>();

            // Products-Categories
            modelBuilder.Entity<Product>()
            .HasMany(i => i.Categories)
            .WithMany(i => i.Products)
            .UsingEntity<ProductCategory>(
                j => j
                    .HasOne(w => w.Category)
                    .WithMany(w => w.ProductCategories)
                    .HasForeignKey(w => w.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(w => w.Product)
                    .WithMany(w => w.ProductCategories)
                    .HasForeignKey(w => w.ProductId)
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.Ignore(w => w.Id).HasKey(w => new { w.ProductId, w.CategoryId });
                });

            // Products-Addons
            modelBuilder.Entity<Product>()
           .HasMany(i => i.Addons)
           .WithMany(i => i.Products)
           .UsingEntity<ProductAddon>(
               j => j
                   .HasOne(w => w.Addon)
                   .WithMany(w => w.ProductAddons)
                   .HasForeignKey(w => w.AddonId)
                   .OnDelete(DeleteBehavior.Cascade),
               j => j
                   .HasOne(w => w.Product)
                   .WithMany(w => w.ProductAddons)
                   .HasForeignKey(w => w.ProductId)
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.Ignore(w => w.Id).HasKey(w => new { w.ProductId, w.AddonId });
               });


        }

    }
}