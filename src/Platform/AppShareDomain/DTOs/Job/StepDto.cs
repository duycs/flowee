using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareDomain.DTOs.Job
{
    public class StepDto : DtoBase
    {
        public int? SpecificationId { get; set; }
        public int? WorkerId { get; set; }
        public int? SkillId { get; set; }
        public int? WorkerSkillLevelId { get; set; }
        public EnumerationDto? WorkerSkillLevel { get; set; }
        public int? SpecificationSkillLevelId { get; set; }
        public EnumerationDto? SpecificationSkillLevel { get; set; }
        public string? Instruction { get; set; }
        public string? Output { get; set; }
        public int? OrderNumber { get; set; }
        public int? OrderOperationNumber { get; set; }
        public int StepStatusId { get; set; }
        public EnumerationDto StepStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
