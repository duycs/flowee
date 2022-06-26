﻿using AppShareServices.DataAccess.Persistences;
using AppShareServices.Events;
using AppShareServices.Models;
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
        public DbSet<WorkerRole> WorkerRoles { get; set; }
        public DbSet<WorkerGroup> WorkerGroups { get; set; }
        public DbSet<WorkerSkill> WorkerSkills { get; set; }
        public DbSet<Shift> Shifts { get; set; }
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

            // Workers-Roles
            modelBuilder.Entity<WorkerRole>().Ignore(w => w.Id).HasKey(w => new { w.WorkerId, w.RoleId });
            modelBuilder.Entity<WorkerRole>()
                .HasOne(w => w.Worker)
                .WithMany(w => w.WorkerRoles)
                .HasForeignKey(w => w.WorkerId);
            modelBuilder.Entity<WorkerRole>()
                .HasOne(w => w.Role)
                .WithMany(w => w.WorkerRoles)
                .HasForeignKey(w => w.WorkerId);

            // Workers-Groups
            modelBuilder.Entity<WorkerGroup>().Ignore(w => w.Id).HasKey(w => new { w.WorkerId, w.GroupId });
            modelBuilder.Entity<WorkerGroup>()
                .HasOne(w => w.Worker)
                .WithMany(w => w.WorkerGroups)
                .HasForeignKey(w => w.WorkerId);
            modelBuilder.Entity<WorkerGroup>()
                .HasOne(w => w.Group)
                .WithMany(w => w.WorkerGroups)
                .HasForeignKey(w => w.GroupId);

            // Workers-Skills
            modelBuilder.Entity<WorkerSkill>().Ignore(w => w.Id).HasKey(w => new { w.WorkerId, w.SkillId });
            modelBuilder.Entity<WorkerSkill>()
                .HasOne(w => w.Worker)
                .WithMany(w => w.WorkerSkills)
                .HasForeignKey(w => w.WorkerId);
            modelBuilder.Entity<WorkerSkill>()
                .HasOne(w => w.Skill)
                .WithMany(w => w.WorkerSkills)
                .HasForeignKey(w => w.SkillId);

            // Groups-Department
            modelBuilder.Entity<Group>().HasOne<Department>(s => s.Department).WithMany(c => c.Groups).HasForeignKey(c => c.DepartmentId);

            // TimeKeeping
            modelBuilder.Entity<TimeKeeping>().HasKey(c => c.Id);
            modelBuilder.Entity<TimeKeeping>().HasOne<Worker>(c => c.Worker).WithMany(c => c.TimeKeepings);
            modelBuilder.Entity<TimeKeeping>().HasOne<Shift>(c => c.Shift).WithMany(c => c.TimeKeepings);
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
