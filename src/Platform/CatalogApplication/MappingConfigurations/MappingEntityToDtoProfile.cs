using AutoMapper;
using CatalogApplication.DTOs;
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
            CreateMap<Catalog, CatalogDto>().ConvertUsing(c => new CatalogDto());
        }
    }
}
