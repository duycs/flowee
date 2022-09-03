using AppShareDomain.Models;
using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Job : Entity, IAggregateRoot
    {
        /// <summary>
        /// A Job defiend status of Product util made done
        /// Product has Specifications(Specification of catalog and addons)
        /// </summary>
        public int ProductId { get; set; }
        public string? Description { get; set; }

        public int JobStatusId { get; set; }
        public JobStatus JobStatus { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public ICollection<int> StepIds { get; set; } = new List<int>();
        public ICollection<Step> Steps { get; set; } = new List<Step>();

        /// <summary>
        /// Statistic
        /// </summary>
        private List<int>? WorkerIds { get; set; }

        public static Job Create(int productId, string? description)
        {
            return new Job()
            {
                ProductId = productId,
                Description = description,
                JobStatus = JobStatus.None,
            };
        }

        public void AddWorker(int workerId)
        {
            WorkerIds?.Add(workerId);
        }

        public void AddWorkers(int[] workerIds)
        {
            WorkerIds?.AddRange(workerIds);
        }

        public void StartJob()
        {
            StartTime = DateTime.UtcNow;
        }

        public void EndJob()
        {
            EndTime = DateTime.UtcNow;
        }

    }
}
