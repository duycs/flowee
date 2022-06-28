using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerApplication.Commands;
using WorkerApplication.ViewModels;

namespace WorkerApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            CreateMap<CreateWorkerVM, CreateWorkerCommand>().ConvertUsing(c => new CreateWorkerCommand(c.FullName, c.Code, c.Email, c.RoleIds, c.GroupIds, c.SkillIds));
            CreateMap<PathUpdateWorkerVM, UpdateWorkerCommand>().ConvertUsing(c => new UpdateWorkerCommand(c.Id, c.FullName, c.RoleIds, c.GroupIds, c.SkillIds));
        }
    }
}
