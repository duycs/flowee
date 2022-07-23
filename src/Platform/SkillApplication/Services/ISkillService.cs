using AppShareDomain.DTOs.Skill;
using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillApplication.Services
{
    public interface ISkillService
    {
        public SkillDto FindSkill(int skillId, bool isInclude = false);
        public List<SkillDto> FindSkills(int[] skillIds, bool isInclude = false);

        /// <summary>
        /// Worker has skill same as specification require skill and has configuration workerLEvelSkill and specificationLevelSkill
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="workerSkillLevelId"></param>
        /// <param name="specificationSkillLevelId"></param>
        /// <returns></returns>
        public List<MatrixSkillDto> FindMatrixSkill(int skillId, int? workerSkillLevelId, int? specificationSkillLevelId);

    }
}
