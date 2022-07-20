using AppShareApplication.MappingConfigurations;
using AutoMapper;

namespace SpecificationApplication.MappingConfigurations
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
                cfg.AddProfile(new MappingSpecificationEntityToDtoProfile());
                cfg.AddProfile(new MappingViewModelToCommandProfile());
                cfg.AddProfile(new MappingViewModelToEntityProfile());
            });
        }
    }
}
