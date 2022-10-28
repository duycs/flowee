using AppShareApplication.Services;
using MediatR;
using OperationApplication.Commands;

namespace OperationApplication.CommandHandlers
{
    public class CommentCommandHandler : IRequestHandler<CommentOperationCommand>
    {
        private readonly IOperationClientService _operationService;
        private readonly ILogClientService _logClientService;


        public Task<Unit> Handle(CommentOperationCommand request, CancellationToken cancellationToken)
        {
            // Find object
            switch (request.CommentForObject)
            {
                case "Operation":
                    var operation = _operationService.Find(request.CommentForObjectId);

                    // fire event insert log for operation

                    break;

                case "Step":

                    // fire event insert log for step

                    break;

                case "Job":

                    // fire event insert log for job

                    break;

                case "Worker":

                    // fire event insert log for worker

                    break;

                default:
                    break;
            }

            return Unit.Task;
        }
    }
}
