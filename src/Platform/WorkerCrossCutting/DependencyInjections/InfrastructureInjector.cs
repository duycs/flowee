using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkerInfrastructure.DataAccess;

namespace WorkerCrossCutting.DependencyInjections
{
    /// <summary>
    /// Database Context, Repository, Queue, Logger
    /// 
    /// </summary>
    public class InfrastructureInjector
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WorkerContext>(options => options.UseMySql(configuration.GetConnectionString("WorkerDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddDbContext<EventContext>(options => options.UseMySql(configuration.GetConnectionString("EventDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddTransient<IDatabaseService, WorkerContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<IDomainEventRepository, DomainEventRepository>();
        }
    }
}
