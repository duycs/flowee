using AutoMapper;

namespace JobApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            //CreateMap<CreateJobVM, CreateJobCommand>().ConvertUsing(c => new CreateJobCommand(c.Code, c.Name, c.RuleIds));
            //CreateMap<UpdateJobVM, UpdateJobCommand>().ConvertUsing(c => new UpdateJobCommand(c.Id, c.Name, c.RuleIds));
        }
    }
}
