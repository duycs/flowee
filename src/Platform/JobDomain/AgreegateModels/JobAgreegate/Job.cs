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
        public Guid ProductId { get; set; }

        public int CatalogId { get; set; }

        public string? Description { get; set; }

        public int JobStatusId { get; set; }
        public JobStatus JobStatus { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public ICollection<int> StepIds { get; set; } = new List<int>();
        public ICollection<Step> Steps { get; set; } = new List<Step>();

        public Step? CurrentStep { get; set; }

        public static Job Create(int catalogId, string? description)
        {
            return new Job()
            {
                ProductId = Guid.NewGuid(),
                CatalogId = catalogId,
                Description = description,
                JobStatus = JobStatus.None,
            };
        }

        public void StartJob()
        {
            JobStatus = JobStatus.Doing;
            StartTime = DateTime.UtcNow;
        }

        public void EndJob()
        {
            JobStatus = JobStatus.Finish;
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
            foreach (var step in Steps)
            {
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
        public Job Transformed(int stepId, out bool isChange)
        {
            isChange = false;

            // Step is trigger
            var step = this.Steps.FirstOrDefault(s => s.Id == stepId);
            if (step is not null && step.Transitions is not null && step.Transitions.Any())
            {
                // Is performed all operations and vaid condition
                var validTransition = step.Transitions.FirstOrDefault(t => t.IsSatisfy());
                if (validTransition is not null && step.IsPerformedAllOperations())
                {
                    this.CurrentStep = validTransition.To();
                    // Set job state by rule

                    isChange = true;
                }
            }

            return this;
        }
    }
}
