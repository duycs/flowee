using AutoMapper;

namespace SkillApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            //CreateMap<CreateSkillVM, CreateSkillCommand>().ConvertUsing(c => new CreateSkillCommand(c.Code, c.Name, c.Description));
            //CreateMap<UpdateSkillVM, UpdateSkillCommand>().ConvertUsing(c => new UpdateSkillCommand(c.Id, c.Code, c.Name, c.Description));
        }
    }
}
