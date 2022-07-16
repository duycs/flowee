using AutoMapper;
using ProductApplication.Commands;
using ProductApplication.ViewModels;

namespace ProductApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            CreateMap<CreateProductVM, CreateProductCommand>().ConvertUsing(c => new CreateProductCommand(c.Code, c.Name, c.Description, c.CatalogId, c.CategoryIds));
            CreateMap<UpdateProductVM, UpdateProductCommand>().ConvertUsing(c => new UpdateProductCommand(c.Id, c.Code, c.Name, c.Description, c.CatalogId, c.CategoryIds));
        }
    }
}
