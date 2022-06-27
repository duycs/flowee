using AppShareServices.DataAccess.Persistences;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Events;
using AppShareServices.Notification;
using MediatR;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerDomain.Events;

namespace WorkerApplication.Commands
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

            // validate roles, groups, skills
            var roles = new List<Role>();
            foreach (var roleId in request.RoleIds)
            {
                var role = _repositoryService.Find<Role>(roleId);
                if (role == null)
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"roleId {roleId} does not exist, new worker could not insert"));
                    return Unit.Value;
                }

                roles.Add(role);
            }

            var groups = new List<Group>();
            foreach (var groupId in request.GroupIds)
            {
                var group = _repositoryService.Find<Group>(groupId);
                if (group == null)
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"groupId {groupId} does not exist, new worker could not insert"));
                    return Unit.Value;
                }

                groups.Add(group);
            }

            var skills = new List<Skill>();
            foreach (var skillId in request.SkillIds)
            {
                var skill = _repositoryService.Find<Skill>(skillId);
                if (skill == null)
                {
                    await _eventDispatcher.RaiseEvent(new DomainNotification(request.MessageType, @$"skillId {skillId} does not exist, new worker could not insert"));
                    return Unit.Value;
                }

                skills.Add(skill);
            }

            var worker = Worker.Create(request.Email, request.Code, request.FullName, roles, groups, skills);
            var workerAdded = _repositoryService.Add<Worker>(worker);
            var result = _repositoryService.SaveChanges();

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
    }
}
