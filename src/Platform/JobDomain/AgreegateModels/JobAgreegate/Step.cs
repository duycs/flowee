using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Step : Entity
    {
        /// <summary>
        /// Step of Job
        /// </summary>
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int SkillId { get; set; }

        /// <summary>
        /// Skill => Operations
        /// </summary>
        public ICollection<Guid>? OperationIds { get; set; }

        /// <summary>
        /// Text instruction all operations
        /// </summary>
        public string? Instruction { get; set; }

        /// <summary>
        /// Woker will be assign to this step
        /// </summary>
        public int? WorkerId { get; set; }

        /// <summary>
        /// Json input
        /// </summary>
        public string? Input { get; set; }

        /// <summary>
        /// Json output
        /// </summary>
        public string? Output { get; set; }

        /// <summary>
        /// Next steps will be set up by who manage the workflow or process mining suggestion
        /// </summary>
        public List<int>? NextStepIds { get; set; }

        /// <summary>
        /// Last steps will be set up by who manage the workflow or process mining suggestion
        /// </summary>
        public List<int>? LastStepIds { get; set; }

        public int StepStatusId { get; set; }
        public StepStatus StepStatus { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public static Step Create(int jobId, int skillId, List<Guid>? operationIds, string? input = "")
        {
            return new Step()
            {
                JobId = jobId,
                SkillId = skillId,
                OperationIds = operationIds,
                Input = input,
                StepStatus = StepStatus.None
            };
        }

        public Step AssignWorker(int workerId)
        {
            WorkerId = workerId;
            return this;
        }

        public void Execute()
        {
            // execute
            Transition(1);
        }

        public void Transition(int nextStepId)
        {
            // validate conditions
        }

        public void SetInstruction(string instruction)
        {
            Instruction = instruction;
        }
    }
}
