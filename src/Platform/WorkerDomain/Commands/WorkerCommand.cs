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
        public int? RoleId { get; set; }
        public int? GroupId { get; set; }
    }

    public sealed class CreateWorkerCommand : WorkerCommand
    {
        public CreateWorkerCommand(string fullName, string? code, string email, int? roleId, int? groupId)
        {
            FullName = fullName;
            Code = code;
            Email = email;
            RoleId = roleId;
            GroupId = groupId;
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
        }
    }
}
