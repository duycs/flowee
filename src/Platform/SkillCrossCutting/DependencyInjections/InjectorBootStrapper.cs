using AppShareServices.Commands;
using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Mappings;
using AppShareServices.Notification;
using AppShareServices.Pagging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillApplication.MappingConfigurations;
using SkillApplication.Services;
using SkillInfrastructure.DataAccess;

namespace SkillCrossCutting.DependencyInjections
{
    public static class InjectorBootStrapper
    {
        public static void AddLayersInjector(this IServiceCollection services, IConfiguration configuration)
        {
            // Application
            services.AddSingleton(AutoMapping.RegisterMappings().CreateMapper());
            services.AddSingleton(sp => sp.GetRequiredService<IMapper>().ConfigurationProvider);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUriService>(o =>
            {
                var request = o.GetRequiredService<IHttpContextAccessor>().HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });
            services.AddTransient<ISkillService, SkillService>();

            // Domain Services
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            //services.AddMediatR(typeof(CatalogCommandHandler).GetTypeInfo().Assembly);
            //services.AddScoped<IRequestHandler<CreateCatalogCommand>, CatalogCommandHandler>();

            // Infrastructure
            services.AddDbContext<SkillContext>(options => options.UseMySql(configuration.GetConnectionString("SkillDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddDbContext<EventContext>(options => options.UseMySql(configuration.GetConnectionString("EventDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddTransient<IDatabaseService, SkillContext>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<IDomainEventRepository, DomainEventRepository>();
            services.AddScoped<IMappingService, MappingService>();
        }
    }
}
