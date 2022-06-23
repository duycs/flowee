using AppShareServices.Pagging;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerCrossCutting.DependencyInjections
{
    /// <summary>
    /// Mapping, Pagging
    /// </summary>
    public class ApplicationInjector
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUriService>(o =>
            {
                var request = o.GetRequiredService<IHttpContextAccessor>().HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                //TODO: https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
                return new UriService(uri);
            });


            // application services
        }
    }
}
