using AppShareServices.Commands;
using FluentValidation;

namespace CatalogApplication.Commands
{
    public abstract class CatalogCommand : Command
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Quantity available in stock
        /// </summary>
        public int QuantityAvailable { get; set; }

        /// <summary>
        /// Price of Product in standar
        /// </summary>
        public decimal PriceStandar { get; set; }

        public int? CurrencyId { get; set; }

        /// <summary>
        /// Specification defiend how to made this Product standar
        /// </summary>
        public int? SpecificationId { get; set; }

        /// <summary>
        /// List of addon for this product
        /// </summary>
        public int[]? AddonIds { get; set; }
    }

    public class CreateCatalogCommand : CatalogCommand
    {
        public CreateCatalogCommand(string code, string name, string? description, int? quantityAvailable,
            decimal? priceStandar, int? currencyId, int? specificationId, int[]? addonIds)
        {
            Code = code;
            Name = name ?? "";
            Description = description ?? "";
            QuantityAvailable = quantityAvailable ?? 0;
            PriceStandar = priceStandar ?? 0;
            CurrencyId = currencyId;
            SpecificationId = specificationId;
            AddonIds = addonIds;
        }

        public override bool IsValid()
        {
            return new CreateCatalogCommandValidator().Validate(this).IsValid;
        }
    }

    public class UpdateCatalogCommand : CatalogCommand
    {
        public UpdateCatalogCommand(int id, string code, string name, string? description, int? quantityAvailable,
            decimal? priceStandar, int? currencyId, int? specificationId, int[]? addonIds)
        {
            Id = id;
            Code = code;
            Name = name ?? "";
            Description = description ?? "";
            QuantityAvailable = quantityAvailable ?? 0;
            PriceStandar = priceStandar ?? 0;
            CurrencyId = currencyId;
            SpecificationId = specificationId;
            AddonIds = addonIds;
        }

        public override bool IsValid()
        {
            return new UpdateCatalogCommandValidator().Validate(this).IsValid;
        }
    }

    public class CreateCatalogCommandValidator : AbstractValidator<CreateCatalogCommand>
    {
        public CreateCatalogCommandValidator()
        {
            RuleFor(i => i.Code).NotEmpty().WithMessage("The code is require");
            RuleFor(i => i.Name).NotEmpty().WithMessage("The name is require");
            RuleFor(i => i.PriceStandar).NotEmpty().WithMessage("The price standar is require");
        }
    }

    public class UpdateCatalogCommandValidator : AbstractValidator<UpdateCatalogCommand>
    {
        public UpdateCatalogCommandValidator()
        {
            RuleFor(i => i.Id).NotEmpty().WithMessage("The id is require");
        }
    }
}

