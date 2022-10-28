using MediatR;
using OperationApplication.Commands;

namespace SpecificationApplication.Commands
{
    public class OperationCommandHandler : IRequestHandler<GetInstructionOperationCommand, string>,
        IRequestHandler<DownloadFileOperationCommand>,
        IRequestHandler<UploadFileOperationCommand>,
        IRequestHandler<ReviewResultOperationCommand>
    {
        public async Task<string> Handle(GetInstructionOperationCommand request, CancellationToken cancellationToken)
        {
            if (request.Specification is null)
            {
                throw new Exception("Specification is null");
            }

            return request.Specification.BuildInstruction().Instruction;
        }

        public async Task<Unit> Handle(DownloadFileOperationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(UploadFileOperationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Unit> Handle(ReviewResultOperationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
