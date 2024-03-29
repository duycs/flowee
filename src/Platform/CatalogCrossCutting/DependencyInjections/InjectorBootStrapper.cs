﻿using AppShareServices.Commands;
using AppShareServices.DataAccess;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Mappings;
using AppShareServices.Notification;
using AppShareServices.Pagging;
using AutoMapper;
using CatalogApplication.ClientServices;
using CatalogApplication.Commands;
using CatalogApplication.MappingConfigurations;
using CatalogApplication.Services;
using CatalogInfrastructure.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Reflection;

namespace CatalogCrossCutting.DependencyInjections
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
            services.AddTransient<ICatalogService, CatalogService>();
            // Ousite Domain Services, ref: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0
            services.AddHttpClient<ISpecificationClientService>();
            // TODO: can not register base address at here?
            //services.AddHttpClient<ISpecificationClientService>(_httpClient =>
            //{
            //    _httpClient.BaseAddress = new Uri($"https://localhost:7174/Specifications/");
            //    _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            //});
            services.AddTransient<ISpecificationClientService, SpecificationClientService>();

            // Domain Services
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddMediatR(typeof(CatalogCommandHandler).GetTypeInfo().Assembly);
            services.AddScoped<IRequestHandler<CreateCatalogCommand>, CatalogCommandHandler>();

            // Infrastructure
            services.AddDbContext<CatalogContext>(options => options.UseMySql(configuration.GetConnectionString("CatalogDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddDbContext<EventContext>(options => options.UseMySql(configuration.GetConnectionString("EventDb"), new MySqlServerVersion(new Version(8, 0, 21))));
            services.AddTransient<IDatabaseService, CatalogContext>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<IDomainEventRepository, DomainEventRepository>();
            services.AddScoped<IMappingService, MappingService>();
        }
    }
}
