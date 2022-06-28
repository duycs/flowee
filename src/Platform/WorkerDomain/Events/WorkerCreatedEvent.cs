using AppShareServices.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerDomain.Events
{
    public class WorkerCreateEvent : DomainEvent
    {
        public Worker Worker { get; set; }

        public override void Flatten()
        {
            Args.Add("WorkerId", Worker.Id);
        }
    }
}
