using System;
using AppShareServices.DataAccess.Persistences;
using Microsoft.EntityFrameworkCore;

namespace AppShareServices.DataAccess
{
    public abstract class DbContextBase<T> : DbContext, IDatabaseService where T : class, IEntityService
    {
        public DbContextBase()
        {
        }

        DbSet<T> IDatabaseService.GetDbSet<T>()
        {
            return Set<T>();
        }

        Task IDatabaseService.SaveChanges()
        {
            return Task.FromResult(base.SaveChanges());
        }
    }
}

