using AppShareServices.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    /// <summary>
    /// JobSteps in Job
    /// 1. Single JobSteps 
    /// 2. Multiple JobSteps run as linked
    /// 3. Multiple JobSteps run as parallel
    /// 4. Can run some JobSteps 1. and 2. and 3.
    /// </summary>
    public class Step : Entity
    {
        /// <summary>
        /// Each Step have a Specification
        /// Input is defiend in specification
        /// </summary>
        public int? SpecificationId { get; set; }

        public int? WorkerId { get; set; }
        public int? SkillId { get; set; }
        //public int? WorkerSkillLevelId { get; set; }
        //public int? SpecificationSkillLevelId { get; set; }

        public string? Instruction { get; set; }

        /// <summary>
        /// TODO: dynamic output
        /// </summary>
        public string? Output { get; set; }

        public int? OrderNumber { get; set; }
        public int? OrderOperationNumber { get; set; }

        public int StepStatusId { get; set; }
        public StepStatus StepStatus { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public static Step Create(int specificationId, int? skillId, string instruction, int orderNumber)
        {
            return new Step()
            {
                SpecificationId = specificationId,
                SkillId = skillId,
                Instruction = instruction,
                OrderNumber = orderNumber,
                StepStatus = StepStatus.None
            };
        }

        public void SetInstruction(string instruction)
        {
            Instruction = instruction;
        }
    }
}
