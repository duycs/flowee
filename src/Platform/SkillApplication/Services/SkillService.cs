using AppShareDomain.DTOs.Skill;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using SkillDomain.AgreegateModels.SkillAgreegate;

namespace SkillApplication.Services
{
    public class SkillService : ISkillService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;

        public SkillService(IRepositoryService repositoryService, IMappingService mappingService)
        {
            _repositoryService = repositoryService;
            _mappingService = mappingService;
        }

        public List<MatrixSkillDto> FindMatrixSkill(int skillId, int? workerSkillLevelId, int? specificationSkillLevelId, bool? isInclude)
        {
            var matrixSkillsExisting = _repositoryService.Find<MatrixSkill>(new MatrixSkillSpecification(isInclude ?? false, skillId, specificationSkillLevelId, workerSkillLevelId)).ToList();
            return _mappingService.Map<List<MatrixSkillDto>>(matrixSkillsExisting);
        }

        public SkillDto FindSkill(int skillId, bool isInclude = false)
        {
            var skillExisting = _repositoryService.Find<Skill>(skillId, new SkillSpecification(isInclude));
            return _mappingService.Map<SkillDto>(skillExisting);
        }

        public List<SkillDto> FindSkills(int[]? ids, bool isInclude = false)
        {
            if (ids is null || !ids.Any())
            {
                return _mappingService.Map<List<SkillDto>>(_repositoryService.Find<Skill>(new SkillSpecification(isInclude)));
            }

            return _mappingService.Map<List<SkillDto>>(_repositoryService.List<Skill>(ids, new SkillSpecification(isInclude)));
        }
    }
}
