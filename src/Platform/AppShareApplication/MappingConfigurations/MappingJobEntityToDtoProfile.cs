using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Operation;
using AutoMapper;
using JobDomain.AgreegateModels.JobAgreegate;
using SpecificationDomain.AgreegateModels.OperationAgreegate;
using State = JobDomain.AgreegateModels.JobAgreegate.State;

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
            CreateMap<State, EnumerationDto>();
        }
    }
}
