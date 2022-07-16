using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using ProductDomain.AgreegateModels.ProductAgreegate;
using System;

namespace ProductApplication.Commands
{
    public class ProductCommandHandler : IRequestHandler<CreateProductCommand>, IRequestHandler<UpdateProductCommand>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventDispatcher _eventDispatcher;

        public ProductCommandHandler(IRepositoryService repositoryService, IEventDispatcher eventDispatcher)
        {
            _repositoryService = repositoryService;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<Category>? categories = null;
            if (request.CategoryIds is not null)
            {
                categories = _repositoryService.List<Category>(request.CategoryIds, out int[] invalidAddons);
                if (invalidAddons.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"invalid CategoryIds {string.Join(",", request.CategoryIds)} does not exist, new product could not insert"));
                    return Unit.Value;
                }
            }

            _repositoryService.Add<Product>(Product.Create(request.Code, request.Name, request.Description, request.CatalogId, categories));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"new product could not insert"));
                return Unit.Value;
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<Category>? categories = null;
            if (request.CategoryIds is not null)
            {
                categories = _repositoryService.List<Category>(request.CategoryIds, out int[] invalidAddons);
                if (invalidAddons.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"invalid CategoryIds {string.Join(",", request.CategoryIds)} does not exist, product could not update"));
                    return Unit.Value;
                }
            }

            var productExisting = _repositoryService.Find<Product>(request.Id);
            if (productExisting is null)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"Product {request.Id} does not exist, product could not update"));
                return Unit.Value;
            }

            _repositoryService.Update<Product>(productExisting.PathUpdate(request.Code, request.Name, request.Description, request.CatalogId, categories));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"product could not update"));
                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}

