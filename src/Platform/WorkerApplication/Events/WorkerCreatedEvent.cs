using AppShareServices.Events;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerApplication.Events
{
    public class WorkerCreatedEvent : DomainEvent
    {
        public Worker Worker { get; set; }

        public override void Flatten()
        {
            Args.Add("WorkerId", Worker.Id);
        }
    }
}
