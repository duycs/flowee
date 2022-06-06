

using AppShareServices.Commands;
using FluentValidation;

namespace AppShareServices.Validations
{
    /// <summary>
    /// Class RemoveCustomerCommandValidator.
    /// </summary>
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        }
    }
}
