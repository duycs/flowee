using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerDomain.AgreegateModels.TimeKeepingAgreegate
{
    public class TimeKeeping : Entity, IAggregateRoot
    {
        public Worker Worker { get; set; }
        public Shift Shift { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeKeeping CheckIn(Worker worker, DateTime startTime)
        {
            return new TimeKeeping()
            {
                Worker = worker,
                StartTime = startTime

            };
        }

        public TimeKeeping CheckOut(Worker worker, DateTime endTime)
        {
            return new TimeKeeping()
            {
                Worker = worker,
                EndTime = endTime
            };
        }
    }
}
