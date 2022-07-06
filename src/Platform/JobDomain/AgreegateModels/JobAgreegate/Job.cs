using AppShareDomain.Models;
using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Job : Entity, IAggregateRoot
    {
        /// <summary>
        /// Product has Specifications for instruction job how to do
        /// </summary>
        public int ProductId { get; set; }
        public int[]? WorkerIds { get; set; }
        public JobStatus JobStatus { get; set; }
        public JobType? JobType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
