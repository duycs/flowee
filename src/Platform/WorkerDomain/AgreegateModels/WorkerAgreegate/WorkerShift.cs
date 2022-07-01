using AppShareServices.Models;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    /// <summary>
    /// No join relation
    /// </summary>
    public class WorkerShift : Entity
    {
        public int WorkerId { get; set; }

        public int ShiftId { get; set; }

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

        public static WorkerShift CreateStartShift(int workerId, int shiftId, DateTime dateStarted)
        {
            return new WorkerShift()
            {
                WorkerId = workerId,
                ShiftId = shiftId,
                DateStarted = dateStarted
            };
        }

        public WorkerShift UpdateStartShift(DateTime dateStarted)
        {
            DateStarted = dateStarted;
            return this;
        }

        public WorkerShift UpdateEndShift(Shift shift, DateTime dateEnded, int? checkinLateHour = 0, int? checkoutEarlyHour = 0)
        {
            DateEnded = dateEnded;
            IsNormal = IsWorkNormalInShift(shift, checkinLateHour, checkoutEarlyHour);
            return this;
        }

        /// <summary>
        /// Check in late and check out early hour or default ignore late/early is normal
        /// </summary>
        /// <param name="shift"></param>
        /// <param name="checkinLateHour"></param>
        /// <param name="checkoutEarlyHour"></param>
        /// <returns></returns>
        public bool IsWorkNormalInShift(Shift shift, int? checkinLateHour = 0, int? checkoutEarlyHour = 0)
        {
            var isNormal = true;

            if (checkinLateHour > 0)
            {
                isNormal = shift.TimeStart.Hour + checkinLateHour < DateStarted.Hour;
            }

            if (checkoutEarlyHour > 0)
            {
                isNormal = isNormal && DateEnded.Hour < shift.TimeEnd.Hour - checkoutEarlyHour;
            }

            return isNormal;
        }

        public bool IsInvalid(int workerId, int shiftId, DateTime dateStarted)
        {
            return WorkerId == workerId && ShiftId == shiftId && DateStarted.Date == dateStarted.Date;
        }
    }
}
