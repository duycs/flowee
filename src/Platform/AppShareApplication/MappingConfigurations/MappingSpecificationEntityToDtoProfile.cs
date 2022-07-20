using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Specification;
using AutoMapper;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareApplication.MappingConfigurations
{
    public class MappingSpecificationEntityToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntityToDtoProfile" /> class.
        /// </summary>
        public MappingSpecificationEntityToDtoProfile()
        {
            // Specification
            CreateMap<Rule, RuleDto>();
            CreateMap<Setting, SettingDto>();
            CreateMap<SettingType, EnumerationDto>();
            CreateMap<Condition, EnumerationDto>();
            CreateMap<Operator, EnumerationDto>();
            CreateMap<Specification, SpecificationDto>();
        }
    }
}
