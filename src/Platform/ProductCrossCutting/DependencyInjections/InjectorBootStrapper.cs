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
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductApplication.Commands;
using ProductApplication.MappingConfigurations;
using ProductApplication.Services;
using ProductInfrastructure.DataAccess;
using System.Reflection;

namespace ProductCrossCutting.DependencyInjections
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
            services.AddTransient<IProductService, ProductService>();

            // Domain
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddMediatR(typeof(ProductCommandHandler).GetTypeInfo().Assembly);
            services.AddScoped<IRequestHandler<CreateProductCommand>, ProductCommandHandler>();

            // Infra
            services.AddDbContext<ProductContext>(options => options.UseMySql(configuration.GetConnectionString("ProductDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddDbContext<EventContext>(options => options.UseMySql(configuration.GetConnectionString("EventDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddTransient<IDatabaseService, ProductContext>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDomainEventRepository, DomainEventRepository>();
            services.AddScoped<IMappingService, MappingService>();

            // Ousite Domain Services
            services.AddHttpClient<ICatalogClientService>();
            services.AddTransient<ICatalogClientService, CatalogClientService>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
        }
    }
}
