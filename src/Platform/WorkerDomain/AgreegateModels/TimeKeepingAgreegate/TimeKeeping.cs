using AppShareServices.Models;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerDomain.AgreegateModels.TimeKeepingAgreegate
{
    public class TimeKeeping : Entity
    {
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        public int ShiftId { get; set; }
        public Shift Shift { get; set; }

        public DateTime DateStarted { get; set; }
        public DateTime DateEnded { get; set; }

        public static TimeKeeping Create(int workerId, int shiftId, DateTime dateStarted, DateTime dateEnded)
        {
            return new TimeKeeping()
            {
                WorkerId = workerId,
                ShiftId = shiftId,
                DateStarted = dateStarted,
                DateEnded = dateEnded
            };
        }
    }
}
