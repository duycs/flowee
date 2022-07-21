using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillApplication.Services
{
    public interface ISkillService
    {
        /// <summary>
        /// Specification has require skill and specificationSkillLevel without workerSkillLevel
        /// This matrixSkill help generate a Step in Job
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="specificationSkillLevelId"></param>
        /// <returns></returns>
        public MatrixSkill FindMatrixSkillSpecificationRequire(int skillId, int specificationSkillLevelId);

        /// <summary>
        /// Worker has skill same as specification require skill and has configuration workerLEvelSkill and specificationLevelSkill
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="workerSkillLevelId"></param>
        /// <param name="specificationSkillLevelId"></param>
        /// <returns></returns>
        public MatrixSkill FindMatchingMatrixSkill(int skillId, int workerSkillLevelId, int specificationSkillLevelId);
    }
}
