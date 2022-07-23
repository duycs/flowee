using AppShareApplication.MappingConfigurations;
using AutoMapper;

namespace SkillApplication.MappingConfigurations
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
                cfg.AddProfile(new MappingSkillEntityToDtoProfile());
                cfg.AddProfile(new MappingViewModelToCommandProfile());
                cfg.AddProfile(new MappingViewModelToEntityProfile());
            });
        }
    }
}
