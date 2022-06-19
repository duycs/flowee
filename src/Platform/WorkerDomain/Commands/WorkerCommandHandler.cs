using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerDomain.Events;

namespace WorkerDomain.Commands
{
    public class WorkerCommandHandler : IRequestHandler<CreateWorkerCommand>
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IUnitOfWork _unitOfWork;

        public WorkerCommandHandler(IRepositoryService repositoryService, IEventDispatcher eventDispatcher, IUnitOfWork unitOfWork)
        {
            _repositoryService = repositoryService;
            _eventDispatcher = eventDispatcher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, request.ValidationResult.ErrorMessage));
                return Unit.Value;
            }

            // validate role
            var role = _repositoryService.Find<Role>(i => i.Id == request.RoleId);
            if (role == null)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"roleId {request.RoleId} does not exist, new worker could not insert"));
                return Unit.Value;
            }

            var worker = Worker.Create(request.Email, request.Code, request.FullName, request.RoleId);
            var workerAdded = _repositoryService.Add(worker);
            var isCommited = _unitOfWork.Commit() > 0;

            if (workerAdded == null || !isCommited)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"new worker could not insert"));
                return Unit.Value;
            }

            // raise created event
            await _eventDispatcher.RaiseEvent(new WorkerCreatedEvent { Worker = workerAdded });

            return Unit.Value;
        }
    }
}
