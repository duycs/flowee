using AppShareDomain.DTOs.Specification;

namespace AppShareDomain.DTOs.Job
{
    public class StepDto : DtoBase
    {
        public int JobId { get; set; }
        public JobDto Job { get; set; }

        public int SkillId { get; set; }

        /// <summary>
        /// Skill => Operations
        /// </summary>
        public List<Guid>? OperationIds { get; set; }
        public List<OperationDto>? Operations { get; set; }

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

        public int? StepStatusId { get; set; }
        public EnumerationDto? StepStatus { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
