using AppShareDomain.DTOs.Skill;

namespace AppShareDomain.DTOs.Worker
{
    public class WorkerSkillDto : DtoBase
    {
        public int WorkerId { get; set; }
        public WorkerDto Worker { get; set; }

        public int? SkillId { get; set; }
        public SkillDto? Skill { get; set; }

        public int? SkillLevelId { get; set; }
        public EnumerationDto? SkillLevel { get; set; }
        public bool IsActive { get; set; }
        public bool IsPriority { get; set; }
    }
}
