using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillApplication.Services
{
    public interface ISkillService
    {
        public MatrixSkill FindMatchingMatrixSkill(int skillId, int workerSkillLevelId, int specificationSkillLevelId);
    }
}
