using AppShareDomain.Models;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using CatalogDomain.AgreegateModels.CatalogAgreegate;
using MediatR;
using System;

namespace CatalogApplication.Commands
{
    public class CatalogCommandHandler : IRequestHandler<CreateCatalogCommand>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IUnitOfWork _unitOfWork;

        public CatalogCommandHandler(IRepositoryService repositoryService, IEventDispatcher eventDispatcher, IUnitOfWork unitOfWork)
        {
            _repositoryService = repositoryService;
            _eventDispatcher = eventDispatcher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateCatalogCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<Addon>? addons = null;
            Currency? currency = null;

            if (request.AddonIds != null)
            {
                addons = _repositoryService.List<Addon>(request.AddonIds, out int[] invalidAddons);
                if (invalidAddons.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"invalidAddonId {string.Join(",", request.AddonIds)} does not exist, new customer could not insert"));
                    return Unit.Value;
                }
            }

            if (request.CurrencyId != null)
            {
                currency = Enumeration.FromValue<Currency>((int)request.CurrencyId);
            }

            _repositoryService.Add<Catalog>(Catalog.Create(request.Code, request.Name, request.PriceStandar, currency, request.SpecificationId, request.QuantityAvailable, request.Description, addons));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"new customer could not insert"));
                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}

