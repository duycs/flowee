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
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
        public int ProductSkillLevelId { get; set; }
        public SpecificationSkillLevel SpecificationSkillLevel { get; set; }
        public int WorkerSkillLevelId { get; set; }
        public WorkerSkillLevel WorkerSkillLevel { get; set; }

        public int ActionId { get; set; }
        public Action Action { get; set; }

        public int ResultId { get; set; }
        public Result Result { get; set; }
    }
}
