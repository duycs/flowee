using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Catalog;
using AutoMapper;
using CatalogDomain.AgreegateModels.CatalogAgreegate;

namespace CatalogApplication.MappingConfigurations
{
    public class MappingEntityToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntityToDtoProfile" /> class.
        /// </summary>
        public MappingEntityToDtoProfile()
        {
            CreateMap<Catalog, CatalogDto>();
            CreateMap<Addon, AddonDto>();
            CreateMap<Currency, EnumerationDto>();
        }
    }
}
