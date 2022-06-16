using AppShareServices.DataAccess.Persistences;
using AppShareServices.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Reflection;
using WorkerDomain.AgreegateModels.TimeKeepingAgreegate;
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
        public DbSet<WorkerSkill> WorkerSkills { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftType> ShiftTypes { get; set; }
        public DbSet<TimeKeeping> TimeKeepings { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public WorkerContext()
        {
        }

        public WorkerContext(DbContextOptions<WorkerContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO:
                optionsBuilder.UseMySql("Server=localhost;port=3306;Database=WorkerDb;user=root;password=abc@123;CharSet=utf8;", new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ShiftTypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SkillLevelEntityTypeConfiguration());
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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
            if (_currentTransaction != null) return null;

            //_currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            _currentTransaction = await Database.BeginTransactionAsync();

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
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
                if (_currentTransaction != null)
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
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
