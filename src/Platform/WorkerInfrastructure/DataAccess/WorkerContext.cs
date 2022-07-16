using AppShareServices.DataAccess.Persistences;
using AppShareServices.Events;
using AppShareServices.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerInfrastructure.DataAccess.EntityConfigurations;

namespace WorkerInfrastructure.DataAccess
{
    public class WorkerContext : DbContext, IDatabaseService
    {
        public const string DEFAULT_SCHEMA = "workerdb";

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<WorkerRole> WorkerRoles { get; set; }
        public DbSet<WorkerGroup> WorkerGroups { get; set; }
        public DbSet<WorkerSkill> WorkerSkills { get; set; }
        public DbSet<WorkerShift> WorkerShifts { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction is not null;


        public WorkerContext(DbContextOptions<WorkerContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public WorkerContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Move to out side
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=WorkerDb;user=root;password=abc@1234;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        /// <summary>
        /// ref: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SkillLevel
            modelBuilder.ApplyConfiguration(new SkillLevelEntityTypeConfiguration());

            // Custom join entity: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#join-entity-type-configuration
            // Workers-Roles
            modelBuilder.Entity<Worker>()
            .HasMany(i => i.Roles)
            .WithMany(i => i.Workers)
            .UsingEntity<WorkerRole>(
                j => j
                    .HasOne(w => w.Role)
                    .WithMany(w => w.WorkerRoles)
                    .HasForeignKey(w => w.RoleId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(w => w.Worker)
                    .WithMany(w => w.WorkerRoles)
                    .HasForeignKey(w => w.WorkerId)
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.Ignore(w => w.Id).HasKey(w => new { w.WorkerId, w.RoleId });
                });

            // Workers-Groups
            modelBuilder.Entity<Worker>()
               .HasMany(i => i.Groups)
               .WithMany(i => i.Workers)
               .UsingEntity<WorkerGroup>(
               j => j
                   .HasOne(w => w.Group)
                   .WithMany(w => w.WorkerGroups)
                   .HasForeignKey(w => w.GroupId)
                   .OnDelete(DeleteBehavior.Cascade),
               j => j
                   .HasOne(w => w.Worker)
                   .WithMany(w => w.WorkerGroups)
                   .HasForeignKey(w => w.WorkerId)
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.Ignore(w => w.Id).HasKey(w => new { w.WorkerId, w.GroupId });
               });

            // Workers-Skills
            modelBuilder.Entity<Worker>()
             .HasMany(i => i.Skills)
             .WithMany(i => i.Workers)
             .UsingEntity<WorkerSkill>(
             j => j
                 .HasOne(w => w.Skill)
                 .WithMany(w => w.WorkerSkills)
                 .HasForeignKey(w => w.SkillId)
                 .OnDelete(DeleteBehavior.Cascade),
             j => j
                 .HasOne(w => w.Worker)
                 .WithMany(w => w.WorkerSkills)
                 .HasForeignKey(w => w.WorkerId)
                 .OnDelete(DeleteBehavior.Cascade),
             j =>
             {
                 j.Ignore(w => w.Id).HasKey(w => new { w.WorkerId, w.SkillId });
             });

            // Workers-Shifts
            modelBuilder.Entity<WorkerShift>().HasKey(w => w.Id);
            modelBuilder.Entity<WorkerShift>().Property(i => i.WorkerId).IsRequired();

            // Groups-Department
            modelBuilder.Entity<Group>().HasOne<Department>(s => s.Department).WithMany(c => c.Groups).HasForeignKey(c => c.DepartmentId);
        }

        public DbSet<T> GetDbSet<T>() where T : class, IEntityService
        {
            return Set<T>();
        }

        Task IDatabaseService.SaveChanges()
        {
            return Task.FromResult(base.SaveChanges());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction?> BeginTransactionAsync()
        {
            if (_currentTransaction is not null) return null;

            //_currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            _currentTransaction = await Database.BeginTransactionAsync();

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction is null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction is not null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction is not null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
