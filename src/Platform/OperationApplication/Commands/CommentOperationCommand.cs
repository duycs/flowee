using AppShareServices.Commands;
using FluentValidation;

namespace OperationApplication.Commands
{
    public class CommentOperationCommand : Command
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public int CommentorId { get; set; }

        public string CommentForObject { get; set; }
        public int CommentForObjectId { get; set; }

        public override bool IsValid()
        {
            return new CommentCommandValidator().Validate(this).IsValid;
        }
    }

    public class CommentCommandValidator : AbstractValidator<CommentOperationCommand>
    {
        public CommentCommandValidator()
        {
            RuleFor(c => c.Body).NotEmpty().WithMessage("The body is require");
            RuleFor(c => c.CommentForObject).NotEmpty().WithMessage("The object is require");
            RuleFor(c => c.CommentForObjectId).NotEmpty().WithMessage("The object id is require");
        }
    }
}
