using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerInfrastructure.CrossCuttingIoC
{
    public static class InjectorBootStrapper
    {
        public static void AddLayersInjector(this IServiceCollection services)
        {
            ApplicationInjector.Register(services);
            DomainInjector.Register(services);
            InfrastructureInjector.Register(services);
        }
    }
}
