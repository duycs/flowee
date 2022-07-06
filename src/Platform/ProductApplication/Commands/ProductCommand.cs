using System;
using AppShareServices.Commands;
using FluentValidation;

namespace ProductApplication.Commands
{
    public abstract class ProductCommand : Command
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? PriceStandar { get; set; }
        public int? QuantityAvailable { get; set; }
        public int? SpecificationId { get; set; }
        public int[]? AddonIds { get; set; }
        public int[]? CategoryIds { get; set; }
    }

    public class CreateProductCommand : ProductCommand
    {
        public CreateProductCommand(string code, string? name, string? description, decimal? priceStandar, int? quantityAvailable, int? specificationId, int[]? addonIds, int[]? categoryIds)
        {
            Code = code;
            Name = name;
            Description = description;
            PriceStandar = priceStandar;
            QuantityAvailable = quantityAvailable;
            SpecificationId = specificationId;
            AddonIds = addonIds;
            CategoryIds = categoryIds;
        }

        public override bool IsValid()
        {
            return new CreateProductCommandValidator().Validate(this).IsValid;
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(i => i.Code).NotEmpty().WithMessage("The code is require");
        }
    }

    public class UpdateProductCommand : ProductCommand
    {
        public UpdateProductCommand(string code, string? name, string? description, decimal? priceStandar, int? quantityAvailable, int? specificationId, int[]? addonIds, int[]? categoryIds)
        {
            Code = code;
            Name = name;
            Description = description;
            PriceStandar = priceStandar;
            QuantityAvailable = quantityAvailable;
            SpecificationId = specificationId;
            AddonIds = addonIds;
            CategoryIds = categoryIds;
        }

        public override bool IsValid()
        {
            return true;
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator() { }
    }
}

