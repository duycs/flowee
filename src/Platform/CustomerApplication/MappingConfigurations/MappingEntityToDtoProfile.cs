using AutoMapper;
using CustomerApplication.DTOs;
using CustomerDomain.AgreegateModels.CustomerAgreegate;

namespace CustomerApplication.MappingConfigurations
{
    public class MappingEntityToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntityToDtoProfile" /> class.
        /// </summary>
        public MappingEntityToDtoProfile()
        {
            CreateMap<Customer, CustomerDto>();
        }
    }
}
