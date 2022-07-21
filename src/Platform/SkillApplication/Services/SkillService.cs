using AppShareServices.DataAccess.Repository;
using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillApplication.Services
{
    public class SkillService : ISkillService
    {
        private readonly IRepositoryService _repositoryService;

        public SkillService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public MatrixSkill FindMatchingMatrixSkill(int skillId, int workerSkillLevelId, int specificationSkillLevelId)
        {
            var matchingMatrixSkill = _repositoryService.Find<MatrixSkill>(m => m.SkillId == skillId
                                        && m.WorkerSkillLevelId == workerSkillLevelId
                                        && m.SpecificationSkillLevelId == specificationSkillLevelId);

            return matchingMatrixSkill;
        }
    }
}
