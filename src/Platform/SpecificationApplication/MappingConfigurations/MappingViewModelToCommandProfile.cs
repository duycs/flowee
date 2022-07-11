using AutoMapper;
using SpecificationApplication.Commands;
using SpecificationApplication.ViewModels;

namespace SpecificationApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            CreateMap<CreateSpecificationVM, CreateSpecificationCommand>().ConvertUsing(c => new CreateSpecificationCommand(c.Code, c.Name, c.RuleIds));
            CreateMap<UpdateSpecificationVM, UpdateSpecificationCommand>().ConvertUsing(c => new UpdateSpecificationCommand(c.Id, c.Name, c.RuleIds));
        }
    }
}
