using AppShareDomain.Models;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using MediatR;

namespace CustomerApplication.Commands
{
    public class CustomerCommandHandler : IRequestHandler<CreateCustomerCommand>, IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerCommandHandler(IRepositoryService repositoryService, IEventDispatcher eventDispatcher, IUnitOfWork unitOfWork)
        {
            _repositoryService = repositoryService;
            _eventDispatcher = eventDispatcher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            List<PaymentMethod>? paymentMethods = null;
            Currency? currency = null;
            PriorityLevel? priorityLevel = null;

            if (request.PaymentMethodIds is not null)
            {
                paymentMethods = _repositoryService.List<PaymentMethod>(request.PaymentMethodIds, out int[] invalidPaymentMethods);
                if (invalidPaymentMethods.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"paymentMethodId {string.Join(",", invalidPaymentMethods)} does not exist, new customer could not insert"));
                    return Unit.Value;
                }
            }

            if (request.CurrencyId is not null)
            {
                currency = Enumeration.FromValue<Currency>((int)request.CurrencyId);
            }

            if (request.PriorityLevelId is not null)
            {
                priorityLevel = Enumeration.FromValue<PriorityLevel>((int)request.PriorityLevelId);
            }

            _repositoryService.Add<Customer>(Customer.Create(request.Email, request.FirstName, request.LastName, request.Phone, request.Description, currency, priorityLevel, paymentMethods));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"new customer could not insert"));
                return Unit.Value;
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            List<PaymentMethod>? paymentMethods = null;
            Currency? currency = null;
            PriorityLevel? priorityLevel = null;

            if (request.PaymentMethodIds is not null)
            {
                paymentMethods = _repositoryService.List<PaymentMethod>(request.PaymentMethodIds, out int[] invalidPaymentMethods);
                if (invalidPaymentMethods.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"paymentMethodId {string.Join(",", invalidPaymentMethods)} does not exist, new customer could not insert"));
                    return Unit.Value;
                }
            }

            if (request.CurrencyId is not null)
            {
                currency = Enumeration.FromValue<Currency>((int)request.CurrencyId);
            }

            if (request.PriorityLevelId is not null)
            {
                priorityLevel = Enumeration.FromValue<PriorityLevel>((int)request.PriorityLevelId);
            }

            var customerExisting = _repositoryService.Find<Customer>(request.Id);

            if(customerExisting is not null)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"customerId {request.Id} does not exist, could not update"));
                return Unit.Value;
            }

            _repositoryService.Update<Customer>(customerExisting.PathUpdate(request.FirstName, request.LastName, request.Phone, request.Description, currency, priorityLevel, paymentMethods));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"customer could not update"));
                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}
