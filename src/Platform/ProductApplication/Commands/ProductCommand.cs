using System;
using AppShareServices.Commands;
using FluentValidation;

namespace ProductApplication.Commands
{
    public abstract class ProductCommand : Command
    {
        public int Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int[]? CategoryIds { get; set; }

        /// <summary>
        /// Catalog has all data of product
        /// </summary>
        public int? CatalogId { get; set; }

        /// <summary>
        /// Instruction description overall how to made this product
        /// Deductive from specifications of catalog
        /// </summary>
        public string? Instruction { get; set; }
    }

    public class CreateProductCommand : ProductCommand
    {
        public CreateProductCommand(string code, string? name, string? description, int catalogId, int[]? categoryIds)
        {
            Code = code;
            Name = name ?? "";
            Description = description ?? "";
            CatalogId = catalogId;
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
        public UpdateProductCommand(int id, string code, string? name, string? description, int? catalogId, int[]? categoryIds)
        {
            Id = id;
            Code = code;
            Name = name ?? "";
            Description = description ?? "";
            CatalogId = catalogId;
            CategoryIds = categoryIds;
        }

        public override bool IsValid()
        {
            return new UpdateProductValidator().Validate(this).IsValid;
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(c => c.Id).NotNull().WithMessage("The Id is require");
        }
    }
}

