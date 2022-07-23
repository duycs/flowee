using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Skill;
using AutoMapper;
using SkillDomain.AgreegateModels.SkillAgreegate;
using Action = SkillDomain.AgreegateModels.SkillAgreegate.Action;

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
            CreateMap<Result, ResultDto>();
            CreateMap<MatrixSkill, MatrixSkillDto>();
        }
    }
}
