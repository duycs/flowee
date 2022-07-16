using AutoMapper;
using CatalogApplication.Commands;
using CatalogApplication.ViewModels;

namespace CatalogApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            CreateMap<CreateCatalogVM, CreateCatalogCommand>().ConvertUsing(c => new CreateCatalogCommand(c.Code, c.Name, c.Description, c.QuantityAvailable, 
                c.PriceStandar, c.CurrencyId, c.SpecificationId, c.AddonIds));
            CreateMap<UpdateCatalogVM, UpdateCatalogCommand>().ConvertUsing(c => new UpdateCatalogCommand(c.Id, c.Code, c.Name, c.Description, c.QuantityAvailable,
            c.PriceStandar, c.CurrencyId, c.SpecificationId, c.AddonIds));
        }
    }
}
