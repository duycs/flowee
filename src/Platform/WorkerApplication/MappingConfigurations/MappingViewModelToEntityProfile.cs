using AutoMapper;
using WorkerApplication.ViewModels;
using WorkerDomain.Commands;

namespace WorkerApplication.MappingConfigurations
{
    public class MappingViewModelToEntityProfile : Profile
    {
        public MappingViewModelToEntityProfile()
        {
            CreateMap<CreateWorkerVM, CreateWorkerCommand>().ConvertUsing(c => new CreateWorkerCommand(c.FullName, c.Code, c.Email, c.RoleId, c.GroupId));
        }
    }
}
