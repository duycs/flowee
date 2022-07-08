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
        public JobStatus JobStatus { get; set; }
        public JobType JobType { get; set; }

        public List<int>? WorkerIds { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public ICollection<JobStep>? JobSteps { get; set; }

        /// <summary>
        /// Notes from Tracking Note Service
        /// </summary>
        public ICollection<int>? NoteIds { get; set; }

        public static Job Create(int productId)
        {
            return new Job()
            {
                ProductId = productId,
                JobStatus = JobStatus.Backlog,
                JobType = JobType.None,
                WorkerIds = new List<int>(),
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

        /// <summary>
        /// Generate JobSteps by Product instructions
        /// </summary>
        /// <returns></returns>
        public List<JobStep> GenerateJobSteps()
        {
            return new List<JobStep>();
        }

    }
}
