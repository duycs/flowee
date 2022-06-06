using AppShareServices.Pagging;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInfrastructure.CrossCuttingIoC
{
    /// <summary>
    /// Mapping, Pagging
    /// </summary>
    public class ApplicationInjector
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                //TODO: https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
                return new UriService(uri);
            });
        }
    }
}
