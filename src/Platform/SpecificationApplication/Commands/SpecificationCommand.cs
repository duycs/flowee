using AppShareServices.Commands;
using FluentValidation;

namespace SpecificationApplication.Commands
{
    public abstract class SpecificationCommand : Command
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] RuleIds { get; set; }
    }

    public class CreateSpecificationCommand : SpecificationCommand
    {
        public CreateSpecificationCommand(string code, string? name, int[]? ruleIds)
        {
            Code = code;
            Name = name;
            RuleIds = ruleIds;
        }

        public override bool IsValid()
        {
            return new CreateSpecificationCommandValidator().Validate(this).IsValid;
        }
    }

    public class CreateSpecificationCommandValidator : AbstractValidator<CreateSpecificationCommand>
    {
        public CreateSpecificationCommandValidator()
        {
            RuleFor(c => c.Code).NotEmpty().WithMessage("The Code is require");
        }
    }

    public class UpdateSpecificationCommand : SpecificationCommand
    {
        public UpdateSpecificationCommand(int id, string? name, string? description, int[]? ruleIds)
        {
            Id = id;
            Name = name;
            RuleIds = ruleIds;
        }

        public override bool IsValid()
        {
            return new UpdateSpecificationCommandValidator().Validate(this).IsValid;
        }
    }

    public class UpdateSpecificationCommandValidator : AbstractValidator<UpdateSpecificationCommand>
    {
        public UpdateSpecificationCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().WithMessage("The Id is require");
        }
    }
}
