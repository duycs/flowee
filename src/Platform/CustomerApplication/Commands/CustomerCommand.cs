using AppShareServices.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApplication.Commands
{
    public abstract class CustomerCommand : Command
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public int? CurrencyId { get; set; }
        public int? PriorityLevelId { get; set; }
        public int[]? PaymentMethodIds { get; set; }
    }

    public class CreateCustomerCommand : CustomerCommand
    {
        public CreateCustomerCommand(string? firstName, string? lastName, string email, string? phone, string? description, int? currencyId, int? priorityLevelId, int[]? paymentMethodIds)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Description = description;
            CurrencyId = currencyId;
            PriorityLevelId = priorityLevelId;
            PaymentMethodIds = paymentMethodIds;
        }

        public override bool IsValid()
        {
            return new CreateCustomerCommandValidator().Validate(this).IsValid;
        }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(i => i.Email).NotEmpty().WithMessage("The email is require");
            RuleFor(i => i.Email).EmailAddress().WithMessage("The email invalid");
        }
    }
}
