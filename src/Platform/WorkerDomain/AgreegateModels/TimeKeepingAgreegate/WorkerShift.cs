using AppShareServices.Models;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerDomain.AgreegateModels.TimeKeepingAgreegate
{
    public class WorkerShift : Entity
    {
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        public int ShiftId { get; set; }
        public Shift Shift { get; set; }

        /// <summary>
        /// Statistic by start shift and end shift => not enought working hours or normal full time working
        /// </summary>
        public bool IsNormal { get; set; }

        public DateTime DateStarted { get; set; }
        public DateTime DateEnded { get; set; }

        public static WorkerShift Create(int workerId, int shiftId, DateTime dateStarted, DateTime dateEnded)
        {
            return new WorkerShift()
            {
                WorkerId = workerId,
                ShiftId = shiftId,
                DateStarted = dateStarted,
                DateEnded = dateEnded
            };
        }
    }
}
