using AppShareServices.Commands;
using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Mappings;
using AppShareServices.Notification;
using AppShareServices.Pagging;
using AppShareServices.Services;
using AutoMapper;
using JobApplication.MappingConfigurations;
using JobApplication.Services;
using JobInfrastructure.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobCrossCutting.DependencyInjections
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
            services.AddHttpClient<ICatalogClientService>();
            services.AddTransient<ICatalogClientService, CatalogClientService>();
            services.AddHttpClient<ISkillClientService>();
            services.AddTransient<ISkillClientService, SkillClientService>();
            services.AddTransient<IJobService, JobService>();

            // Domain
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            //services.AddMediatR(typeof(JobCommandHandler).GetTypeInfo().Assembly);
            //services.AddScoped<IRequestHandler<CreateJobCommand>, JobCommandHandler>();

            // Infrastructure
            services.AddDbContext<JobContext>(options => options.UseMySql(configuration.GetConnectionString("JobDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddDbContext<EventContext>(options => options.UseMySql(configuration.GetConnectionString("EventDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddTransient<IDatabaseService, JobContext>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<IDomainEventRepository, DomainEventRepository>();
            services.AddScoped<IMappingService, MappingService>();
        }
    }
}
