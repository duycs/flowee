using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using WorkerApplication.Events;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerDomain.Events;

namespace WorkerApplication.Commands
{
    public class WorkerCommandHandler : IRequestHandler<CreateWorkerCommand>, IRequestHandler<UpdateWorkerCommand>
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

            // Validate roles, groups, skills
            // Can nullable for not insert
            List<Role>? roles = null;
            List<Group>? groups = null;
            List<Skill>? skills = null;
            if (request.RoleIds != null)
            {
                roles = _repositoryService.List<Role>(request.RoleIds, out int[] invalidRoleIds);
                if (invalidRoleIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"roleId {string.Join(",", invalidRoleIds)} does not exist, new worker could not insert"));
                    return Unit.Value;
                }
            }

            if (request.GroupIds != null)
            {
                groups = _repositoryService.List<Group>(request.GroupIds, out int[] invalidGroupIds);
                if (invalidGroupIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"groupId {string.Join(",", invalidGroupIds)} does not exist, new worker could not insert"));
                    return Unit.Value;
                }
            }

            if (request.SkillIds != null)
            {
                skills = _repositoryService.List<Skill>(request.SkillIds, out int[] invalidSkillIds);
                if (invalidSkillIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"skillId {string.Join(",", invalidSkillIds)} does not exist, new worker could not insert"));
                    return Unit.Value;
                }
            }

            var workerAdded = _repositoryService.Add<Worker>(Worker.Create(request.Email, request.Code, request.FullName, roles, groups, skills));
            var result = _repositoryService.SaveChanges();

            // TODO: add then save change but DateCreated of relations entity is default

            // TODO: unitOfWork return false
            //var isCommited = _unitOfWork.Commit() > 0;

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"new worker could not insert"));
                return Unit.Value;
            }

            // raise created event
            await _eventDispatcher.RaiseEvent(new WorkerCreatedEvent { Worker = workerAdded });
            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
        {
            // Validate roles, groups, skills
            // Can nullable for not update
            List<Role>? roles = null;
            List<Group>? groups = null;
            List<Skill>? skills = null;
            if (request.RoleIds != null)
            {
                roles = _repositoryService.List<Role>(request.RoleIds, out int[] invalidRoleIds);
                if (invalidRoleIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"roleId {string.Join(",", invalidRoleIds)} does not exist, new worker could not insert"));
                    return Unit.Value;
                }
            }

            if (request.GroupIds != null)
            {
                groups = _repositoryService.List<Group>(request.GroupIds, out int[] invalidGroupIds);
                if (invalidGroupIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"groupId {string.Join(",", invalidGroupIds)} does not exist, new worker could not insert"));
                    return Unit.Value;
                }
            }

            if (request.SkillIds != null)
            {
                skills = _repositoryService.List<Skill>(request.SkillIds, out int[] invalidSkillIds);
                if (invalidSkillIds.Any())
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"skillId {string.Join(",", invalidSkillIds)} does not exist, new worker could not insert"));
                    return Unit.Value;
                }
            }

            var workerExsiting = _repositoryService.Find<Worker>(request.Id);
            if (workerExsiting == null)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"worker does not existing"));
                return Unit.Value;
            }

            // TODO: wrong update relations table: not override, not found relations data in tables
            var workerUpdated = _repositoryService.Update(workerExsiting.PathUpdateWorker(request.FullName, roles, groups, skills));
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @"worker could not update"));
                return Unit.Value;
            }

            // raise updated event
            return Unit.Value;
        }
    }
}
