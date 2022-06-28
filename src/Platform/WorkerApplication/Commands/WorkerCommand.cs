using AppShareServices.Commands;
using FluentValidation;

namespace WorkerApplication.Commands
{
    // Abstract command
    public abstract class WorkerCommand : Command
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public int[]? RoleIds { get; set; }
        public int[]? GroupIds { get; set; }
        public int[]? SkillIds { get; set; }
    }

    // Commands
    public sealed class CreateWorkerCommand : WorkerCommand
    {
        public CreateWorkerCommand(string fullName, string? code, string email, int[]? roleIds, int[]? groupIds, int[]? skillIds)
        {
            FullName = fullName;
            Code = code;
            Email = email;
            RoleIds = roleIds;
            GroupIds = groupIds;
            SkillIds = skillIds;
        }

        public override bool IsValid()
        {
            return new CreateWorkerCommandValidator().Validate(this).IsValid;
        }
    }

    public sealed class UpdateWorkerCommand : WorkerCommand
    {
        public UpdateWorkerCommand(int id, string? fullName, int[]? roleIds, int[]? groupIds, int[]? skillIds)
        {
            Id = id;
            FullName = fullName;
            RoleIds = roleIds;
            GroupIds = groupIds;
            SkillIds = skillIds;
        }

        public override bool IsValid()
        {
            return new UpdateWorkerCommandValidator().Validate(this).IsValid;
        }
    }

    // Validates
    public class CreateWorkerCommandValidator : AbstractValidator<CreateWorkerCommand>
    {
        public CreateWorkerCommandValidator()
        {
            RuleFor(i => i.Email).NotEmpty().WithMessage("The email is required");
            RuleFor(i => i.Email).EmailAddress().WithMessage("The email is invalid");
        }
    }

    public class UpdateWorkerCommandValidator : AbstractValidator<UpdateWorkerCommand>
    {
        public UpdateWorkerCommandValidator()
        {
            RuleFor(i => i.Id).NotEmpty().WithMessage("The id is required");
        }
    }
}
