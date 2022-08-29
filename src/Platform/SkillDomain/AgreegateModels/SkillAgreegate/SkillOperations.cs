using AppShareDomain.Models;
using AppShareServices.Models;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class SkillOperations : Entity
    {
        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public Guid OperationGuid { get; set; }
    }
}
