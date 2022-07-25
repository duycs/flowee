using AppShareDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.DataAccess.Persistences
{
    public interface IDatabaseService
    {
        DbSet<T> GetDbSet<T>() where T : EntityBase;
        Task SaveChanges();
    }
}
