using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Product;
using AppShareDomain.DTOs.Specification;
using AutoMapper;
using CatalogDomain.AgreegateModels.CatalogAgreegate;
using ProductDomain.AgreegateModels.ProductAgreegate;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace CatalogApplication.MappingConfigurations
{
    public class MappingEntityToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingEntityToDtoProfile" /> class.
        /// </summary>
        public MappingEntityToDtoProfile()
        {
            // Catalog
            CreateMap<Catalog, CatalogDto>();
            CreateMap<Addon, AddonDto>();
            CreateMap<Currency, EnumerationDto>();

            // Specification
            CreateMap<Specification, SpecificationDto>();
            CreateMap<Rule, RuleDto>();
            CreateMap<Setting, SettingDto>();
            CreateMap<SettingType, EnumerationDto>();

            // Product
            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();

            // 
        }
    }
}
