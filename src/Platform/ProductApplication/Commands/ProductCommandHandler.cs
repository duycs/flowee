using MediatR;
using System;

namespace ProductApplication.Commands
{
	public class ProductCommandHandler : IRequestHandler<CreateProductCommand>
	{
		public ProductCommandHandler()
		{
		}

        public Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

