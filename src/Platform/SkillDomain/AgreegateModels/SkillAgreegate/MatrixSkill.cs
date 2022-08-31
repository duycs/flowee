using AppShareServices.Models;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    /// <summary>
    /// Statistic field by skill
    /// </summary>
    public class MatrixSkill : Entity
    {
        /// <summary>
        /// Worker and Specification same as Skill
        /// </summary>
        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public int SpecificationSkillLevelId { get; set; }
        public SpecificationSkillLevel SpecificationSkillLevel { get; set; }

        public int WorkerSkillLevelId { get; set; }
        public WorkerSkillLevel WorkerSkillLevel { get; set; }

        /// <summary>
        /// EstimationTime in mini second
        /// </summary>
        public int EstimationTimeInMiniSecond { get; set; }
    }
}
