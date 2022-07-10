using AppShareDomain.Models;
using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Job : Entity, IAggregateRoot
    {
        /// <summary>
        /// A Job defiend status of Product util made done
        /// Product has Specifications for instruction job how to do
        /// </summary>
        public int ProductId { get; set; }

        private List<int> WorkerIds { get; set; }

        /// <summary>
        /// Have 4 steps and 4 workers need to do in a Job, the parallel scenario example:
        /// 1. First time  worker 1 do Step 1, output of step 1 is input of step 2
        /// 2. Worker 2 do Step 2 and worker 3 do step 3 at the same time, output of steps 2 and 3 is input of step 4
        /// 3. Worker 4 do step 4 at final, output of step 4 is final result.
        /// </summary>
        public ICollection<Operation>? Operations { get; set; }

        public int JobStatusId { get; set; }
        public JobStatus JobStatus { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public static Job Create(int productId)
        {
            return new Job()
            {
                ProductId = productId,
                JobStatus = JobStatus.None,
            };
        }

        /// <summary>
        /// Order Scenarios
        /// </summary>
        /// <returns></returns>
        public List<Operation> SortOperations()
        {
            return Operations.OrderBy(s => s.OrderNumber).ToList();
        }

        /// <summary>
        /// Some JobSteps run single, some run linked, some run parallel
        /// </summary>
        /// <returns></returns>
        public string GetJobStepsOperation()
        {
            return "";
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
        public List<Step> GenerateJobSteps()
        {
            return new List<Step>();
        }

    }
}
