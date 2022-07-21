using AppShareServices.Models;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class SpecificationSkill : Entity
    {
        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }

        public int? SkillId { get; set; }
        public int? SkillLevelId { get; set; }
    }
}
