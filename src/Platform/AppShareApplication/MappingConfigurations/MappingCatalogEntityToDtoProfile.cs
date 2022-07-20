using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Catalog;
using AutoMapper;
using CatalogDomain.AgreegateModels.CatalogAgreegate;

namespace AppShareApplication.MappingConfigurations
{
    public class MappingCatalogEntityToDtoProfile : Profile
    {
        public MappingCatalogEntityToDtoProfile()
        {
            CreateMap<Catalog, CatalogDto>();
            CreateMap<Addon, AddonDto>();
            CreateMap<Currency, EnumerationDto>();
        }
    }
}
