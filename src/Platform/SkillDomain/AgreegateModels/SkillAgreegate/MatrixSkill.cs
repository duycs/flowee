using AppShareServices.Models;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    /// <summary>
    /// Matrix Skills
    /// Skill need to do specifications of product 
    /// and skill worker nice to have 
    /// => Action to do => Expect result done product
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

        public int ActionId { get; set; }
        /// <summary>
        /// Action worker should do
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// EstimationTime in mini second
        /// </summary>
        public int EstimationTimeInMiniSecond { get; set; }

        public int ResultId { get; set; }
        /// <summary>
        /// Expect result
        /// </summary>
        public Result Result { get; set; }
    }
}
