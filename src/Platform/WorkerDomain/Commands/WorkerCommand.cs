using AppShareServices.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkerDomain.Commands
{
    public abstract class WorkerCommand : Command
    {
        public string? FullName { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> GroupIds { get; set; }
        public List<int> SkillIds { get; set; }
    }

    public sealed class CreateWorkerCommand : WorkerCommand
    {
        public CreateWorkerCommand(string fullName, string? code, string email, List<int>? roleIds, List<int>? groupIds, List<int>? skillIds)
        {
            FullName = fullName;
            Code = code;
            Email = email;
            RoleIds = roleIds ?? new List<int>();
            GroupIds = groupIds ?? new List<int>();
            SkillIds = skillIds ?? new List<int>();
        }

        public override bool IsValid()
        {
            return new CreateWorkerCommandValidator().Validate(this).IsValid;
        }
    }

    public class CreateWorkerCommandValidator : AbstractValidator<CreateWorkerCommand>
    {
        public CreateWorkerCommandValidator()
        {
            RuleFor(i => i.Email).NotEmpty().WithMessage("The email is required");
            RuleFor(i => i.Email).EmailAddress().WithMessage("The email is invalid");
        }
    }
}
