using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerInfrastructure.DataAccess;

namespace WorkerInfrastructure.CrossCuttingIoC
{
    /// <summary>
    /// Database Context, Repository, Queue, Logger
    /// 
    /// </summary>
    public class InfrastructureInjector
    {
        public static void Register(IServiceCollection services)
        {
            services.AddDbContext<WorkerContext>();
            services.AddTransient<IDatabaseService, WorkerContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepositoryService, RepositoryService>();
        }
    }
}
