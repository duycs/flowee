using AppShareApplication.Services;
using MediatR;
using OperationApplication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationApplication.CommandHandlers
{
    public class ReviewResultCommandHandler : IRequestHandler<ReviewResultOperationCommand>
    {
        private readonly IJobClientService _jobClientService;

        public ReviewResultCommandHandler(IJobClientService jobClientService)
        {
            _jobClientService = jobClientService;
        }

        public Task<Unit> Handle(ReviewResultOperationCommand request, CancellationToken cancellationToken)
        {
            switch (request.SubmitType)
            {
                case SubmitType.Approved:
                    break;

                case SubmitType.Rejected:
                    foreach (var stepId in request.RejectToSteps)
                    {
                        var stepDto = _jobClientService.GetStep(stepId);
                        // update step rejected

                        _jobClientService.GoToStep(stepId, request.Comment);
                    }
                    break;

                default:
                    break;
            }

            return Unit.Task;
        }

    }
}
