using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Skill;
using AutoMapper;
using SkillDomain.AgreegateModels.SkillAgreegate;

namespace AppShareApplication.MappingConfigurations
{
    public class MappingSkillEntityToDtoProfile : Profile
    {
        public MappingSkillEntityToDtoProfile()
        {
            CreateMap<SpecificationSkillLevel, EnumerationDto>();
            CreateMap<WorkerSkillLevel, EnumerationDto>();
            CreateMap<Skill, SkillDto>();
            CreateMap<Action, ActionDto>();
        }
    }
}
