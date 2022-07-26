using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Skill;
using AppShareDomain.DTOs.Worker;
using AutoMapper;
using SkillDomain.AgreegateModels.SkillAgreegate;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerApplication.MappingConfigurations
{
    public class MappingEntityToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntityToDtoProfile" /> class.
        /// </summary>
        public MappingEntityToDtoProfile()
        {
            CreateMap<Worker, WorkerDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<WorkerSkill, WorkerSkillDto>();
            CreateMap<Skill, SkillDto>();
            CreateMap<WorkerSkillLevel, EnumerationDto>();
        }
    }
}
