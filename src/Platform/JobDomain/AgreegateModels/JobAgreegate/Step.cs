using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string? Instruction { get; set; }

        /// <summary>
        /// TODO: dynamic output
        /// </summary>
        public string? Output { get; set; }

        public int OrderNumber { get; set; }
        public StepStatus StepStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }


        public void SetInstruction(string instruction)
        {
            Instruction = instruction;
        }
    }
}
