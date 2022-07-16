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
    public class CatalogCommandHandler : IRequestHandler<CreateCatalogCommand>, IRequestHandler<UpdateCatalogCommand>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventDispatcher _eventDispatcher;

        public CatalogCommandHandler(IRepositoryService repositoryService, IEventDispatcher eventDispatcher)
        {
            _repositoryService = repositoryService;
            _eventDispatcher = eventDispatcher;
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

            if (request.AddonIds is not null)
            {
                addons = _repositoryService.List<Addon>(request.AddonIds, out int[] invalidAddons);
                if (invalidAddons.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"invalid AddonIds {string.Join(",", request.AddonIds)} does not exist, new catalog could not insert"));
                    return Unit.Value;
                }
            }

            if (request.CurrencyId is not null)
            {
                currency = Enumeration.FromValue<Currency>((int)request.CurrencyId);
            }

            _repositoryService.Add<Catalog>(Catalog.Create(request.Code, request.Name, request.PriceStandar, currency, request.SpecificationId, request.QuantityAvailable, request.Description, addons));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"new catalog could not insert"));
                return Unit.Value;
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateCatalogCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<Addon>? addons = null;
            Currency? currency = null;

            if (request.AddonIds is not null)
            {
                addons = _repositoryService.List<Addon>(request.AddonIds, out int[] invalidAddons);
                if (invalidAddons.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"invalid AddonIds {string.Join(",", request.AddonIds)} does not exist, catalog could not update"));
                    return Unit.Value;
                }
            }

            if (request.CurrencyId is not null)
            {
                currency = Enumeration.FromValue<Currency>((int)request.CurrencyId);
            }

            var catalogExisting = _repositoryService.Find<Catalog>(request.Id);
            if (catalogExisting is null)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"catalog {request.Id} does not exist"));
                return Unit.Value;
            }

            _repositoryService.Update<Catalog>(catalogExisting.PathUpdate(request.Code, request.Name, request.PriceStandar, currency, request.SpecificationId, request.QuantityAvailable, request.Description, addons));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"catalog could not update"));
                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}

