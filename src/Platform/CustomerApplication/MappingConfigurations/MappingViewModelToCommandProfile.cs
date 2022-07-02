using AutoMapper;
using CustomerApplication.Commands;
using CustomerApplication.ViewModels;

namespace CustomerApplication.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingViewModelToCommandProfile"/> class.
        /// </summary>
        public MappingViewModelToCommandProfile()
        {
            CreateMap<CreateCustomerVM, CreateCustomerCommand>().ConvertUsing(c => new CreateCustomerCommand(c.FirstName, c.LastName, c.Email, c.Phone, c.Description, c.CurrencyId, c.PriorityLevelId, c.PaymentMethodIds));
        }
    }
}
