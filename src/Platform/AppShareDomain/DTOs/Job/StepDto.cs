using AppShareDomain.DTOs.Operation;
using AppShareDomain.DTOs.Skill;
using AppShareDomain.DTOs.Specification;

namespace AppShareDomain.DTOs.Job
{
    public class StepDto : DtoBase
    {
        public int JobId { get; set; }
        public JobDto Job { get; set; }

        public int SkillId { get; set; }
        public SkillDto Skill { get; set; }

        /// <summary>
        /// Skill => Operations
        /// </summary>
        public ICollection<Guid>? OperationIds { get; set; }
        public ICollection<OperationDto>? Operations { get; set; }

        public ICollection<StepOperationDto>? StepOperations { get; set; }

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

        public ICollection<TransitionDto>? Transitions { get; set; }

        public int? StateId { get; set; }
        public EnumerationDto? State { get; set; }

        public bool IsCurrentState { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
