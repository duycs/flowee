using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace SpecificationApplication.Commands
{
    public class SpecificationCommandHandler : IRequestHandler<CreateSpecificationCommand>, IRequestHandler<UpdateSpecificationCommand>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventDispatcher _eventDispatcher;

        public SpecificationCommandHandler(IRepositoryService repositoryService, IEventDispatcher eventDispatcher)
        {
            _repositoryService = repositoryService;
            _eventDispatcher = eventDispatcher;
        }

        public async Task<Unit> Handle(CreateSpecificationCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<Rule>? rules = null;

            if (request.RuleIds is not null)
            {
                rules = _repositoryService.List<Rule>(request.RuleIds, out int[] invalidSettingIds);
                if (invalidSettingIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"Invalid RuleIds {string.Join(",", request.RuleIds)} does not exist, new customer could not insert"));
                    return Unit.Value;
                }
            }

            _repositoryService.Add<Specification>(Specification.Create(request.Code, request.Name, rules));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"New Specification could not insert"));
                return Unit.Value;
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateSpecificationCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<Rule>? rules = null;

            if (request.RuleIds is not null)
            {
                rules = _repositoryService.List<Rule>(request.RuleIds, out int[] invalidRuleIds);
                if (invalidRuleIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"Invalid RuleIds {string.Join(",", request.RuleIds)} does not exist, new customer could not insert"));
                    return Unit.Value;
                }
            }

            var specificationExisting = _repositoryService.Find<Specification>(request.Id);
            if (specificationExisting is null)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"Specification does not existing"));
                return Unit.Value;
            }

            _repositoryService.Update<Specification>(specificationExisting.PathUpdate(request.Name, rules));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"Specification could not update"));
                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}
