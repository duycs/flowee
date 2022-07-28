using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AutoMapper;
using JobDomain.AgreegateModels.JobAgreegate;

namespace AppShareApplication.MappingConfigurations
{
    public class MappingJobEntityToDtoProfile : Profile
    {
        public MappingJobEntityToDtoProfile()
        {
            CreateMap<Job, JobDto>();
            CreateMap<Step, StepDto>();
            CreateMap<Operation, OperationDto>();
            CreateMap<JobStatus, EnumerationDto>();
            CreateMap<StepStatus, EnumerationDto>();
        }
    }
}
