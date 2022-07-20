using AppShareDomain.DTOs.Product;
using AutoMapper;
using ProductDomain.AgreegateModels.ProductAgreegate;

namespace AppShareApplication.MappingConfigurations
{
    public class MappingProductEntityToDtoProfile : Profile
    {
        public MappingProductEntityToDtoProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
