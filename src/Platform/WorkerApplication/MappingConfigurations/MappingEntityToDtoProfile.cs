using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerApplication.DTOs;
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
        }
    }
}
