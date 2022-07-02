using AppShareDomain.Models;
using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApplication.Commands
{
    public class CustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
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

            if (request.PaymentMethodIds != null)
            {
                paymentMethods = _repositoryService.List<PaymentMethod>(request.PaymentMethodIds, out int[] invalidPaymentMethods);
                if (invalidPaymentMethods.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"paymentMethodId {string.Join(",", invalidPaymentMethods)} does not exist, new customer could not insert"));
                    return Unit.Value;
                }
            }

            if (request.CurrencyId != null)
            {
                currency = Enumeration.FromValue<Currency>((int)request.CurrencyId);
            }

            if (request.PriorityLevelId != null)
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
    }
}
