using MediatR;
using OperationApplication.Commands;

namespace OperationApplication.CommandHandlers
{
    public class CommentCommandHandler : IRequestHandler<CommentOperationCommand>
    {
        public Task<Unit> Handle(CommentOperationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
