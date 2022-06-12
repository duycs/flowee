using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerApplication.MappingConfigurations
{
    public class AutoMapping
    {
        /// <summary>
        /// Registers the mappings.
        /// </summary>
        /// <returns>MapperConfiguration.</returns>
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingEntityToDtoProfile());
                cfg.AddProfile(new MappingViewModelToCommandProfile());
                cfg.AddProfile(new MappingViewModelToEntityProfile());
            });
        }
    }
}
