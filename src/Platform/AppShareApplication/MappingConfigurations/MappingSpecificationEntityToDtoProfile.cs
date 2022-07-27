using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Skill;
using AppShareDomain.DTOs.Specification;
using AutoMapper;
using SkillDomain.AgreegateModels.SkillAgreegate;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace AppShareApplication.MappingConfigurations
{
    public class MappingSpecificationEntityToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntityToDtoProfile" /> class.
        /// </summary>
        public MappingSpecificationEntityToDtoProfile()
        {
            CreateMap<Rule, RuleDto>();
            CreateMap<Setting, SettingDto>();
            CreateMap<SettingType, EnumerationDto>();
            CreateMap<Condition, EnumerationDto>();
            CreateMap<Operator, EnumerationDto>();
            CreateMap<Specification, SpecificationDto>();
            CreateMap<Skill, SkillDto>();
            CreateMap<SpecificationSkill, SpecificationSkillDto>();
            CreateMap<SpecificationSkillLevel, EnumerationDto>();
        }
    }
}
