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

        public Step? CurrentStep { get; set; }

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

        /// <summary>
        /// Chain steps from root step
        /// </summary>
        /// <param name="fromStepId"></param>
        /// <returns></returns>
        public List<List<Step>>? GetChainSteps(int rootStepId)
        {
            var chains = new List<List<Step>>();
            foreach(var step in Steps) {
                var nextSteps = step.GetNextSteps();

                // find next steps
            }
            return chains;
        }

        /// <summary>
        /// Transformed a step
        /// </summary>
        /// <param name="stepId"></param>
        /// <param name="outputOperation"></param>
        /// <returns></returns>
        public Job Transformed(int stepId, string outputOperation, out bool isChange)
        {
            isChange = false;
            var step = this.Steps.FirstOrDefault(s => s.Id == stepId);
            if (step is not null && step.Transitions is not null && step.Transitions.Any())
            {
                var validTransition = step.Transitions.FirstOrDefault(t => t.IsValidCondition(outputOperation));
                if (validTransition is not null)
                {
                    this.CurrentStep = validTransition.To();
                    isChange = true;
                }
            }

            return this;
        }

    }
}
