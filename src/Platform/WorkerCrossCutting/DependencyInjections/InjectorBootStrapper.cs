using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerCrossCutting.DependencyInjections
{
    public static class InjectorBootStrapper
    {
        public static void AddLayersInjector(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationInjector.Register(services);
            DomainInjector.Register(services);
            InfrastructureInjector.Register(services, configuration);
        }
    }
}
