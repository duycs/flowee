using AppShareServices.Pagging;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using WorkerApplication.MappingConfigurations;
using WorkerApplication.Services;

namespace WorkerCrossCutting.DependencyInjections
{
    /// <summary>
    /// Mapping, Pagging
    /// </summary>
    public class ApplicationInjector
    {
        public static void Register(IServiceCollection services)
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
            services.AddTransient<IWorkerService, WorkerService>();
            services.AddHttpClient<ISkillClientService>();
            services.AddTransient<ISkillClientService, SkillClientService>();

        }
    }
}
