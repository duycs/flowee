using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerApplication.ViewModels;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerApplication.MappingConfigurations
{
    public class MappingViewModelToEntityProfile : Profile
    {
        public MappingViewModelToEntityProfile()
        {
            CreateMap<WorkerVM, Worker>();
        }
    }
}
