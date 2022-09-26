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
            // Submit Approved
            if (request.SubmitType.Equals(SubmitType.Approved))
            {
                // Update to go next step
                return Unit.Task;
            }

            // Submit Rejected
            if (request.SubmitType.Equals(SubmitType.Rejected))
            {
                // Update to back step

                foreach (var stepId in request.RejectToSteps)
                {
                    var stepDto = _jobClientService.GetStep(stepId);
                    // update step rejected
                }

                return Unit.Task;
            }

            return Unit.Task;
        }
    }
}
