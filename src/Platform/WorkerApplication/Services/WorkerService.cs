using AppShareDomain.DTOs.Worker;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using AppShareServices.Services;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerApplication.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ISkillClientService _skillClientService;
        private readonly IMappingService _mappingService;

        public WorkerService(IRepositoryService repositoryService, ISkillClientService skillClientService, IMappingService mappingService)
        {
            _repositoryService = repositoryService;
            _skillClientService = skillClientService;
            _mappingService = mappingService;
        }

        public async Task<WorkerDto?> Find(int id, bool isInclude)
        {
            var workerExisting = _repositoryService.Find<Worker>(id, new WorkerSpecification(isInclude));
            var workerDto = _mappingService.Map<WorkerDto>(workerExisting);
            if (workerDto is not null && workerDto.WorkerSkills is not null && workerDto.WorkerSkills.Any())
            {
                var skillIds = workerDto.WorkerSkills.Where(s => s.SkillId.HasValue && s.SkillId > 0).Select(s => s.SkillId).Cast<int>().ToArray();
                var skillLevelIds = workerDto.WorkerSkills.Where(s => s.SkillLevelId.HasValue && s.SkillLevelId > 0).Select(s => s.SkillLevelId).Cast<int>().ToArray();
                var skillDtos = await _skillClientService.Get(skillIds, false);
                var workerSkillLevelDtos = await _skillClientService.GetWorkerSkillLevels(skillLevelIds, false);

                workerDto.WorkerSkills.ForEach(workerSkill =>
                {
                    workerSkill.Skill = skillDtos.FirstOrDefault(s => s.Id == workerSkill.SkillId);
                    workerSkill.SkillLevel = workerSkillLevelDtos.FirstOrDefault(s => s.Id == workerSkill.SkillLevelId);
                });
            }

            return workerDto;
        }

        public async Task<IEnumerable<WorkerDto>?> Find(int[] ids, bool isInclude)
        {
            var workersExisting = _repositoryService.List<Worker>(ids, new WorkerSpecification(isInclude));
            var workerDtos = _mappingService.Map<List<WorkerDto>>(workersExisting);
            if (isInclude)
            {
                workerDtos = await FindInclude(workerDtos);
            }

            return workerDtos;
        }

        public List<WorkerDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords)
        {
            var workerSpecification = new WorkerSpecification(isInclude, searchValue, columnOrders.ToColumnOrders());
            var pagedWorkers = _repositoryService.Find<Worker>(pageNumber, pageSize, workerSpecification, out totalRecords).ToList();
            var workerDtos = _mappingService.Map<List<WorkerDto>>(pagedWorkers);
            return FindInclude(workerDtos).Result;
        }

        public async Task<List<WorkerDto>> FindInclude(List<WorkerDto> workerDtos)
        {
            if (workerDtos.Any())
            {
                var skillIds = workerDtos.SelectMany(s => s.WorkerSkills.Where(i => i.SkillId.HasValue && i.SkillId > 0).Select(i => i.SkillId)).Cast<int>().ToArray();
                var skillLevelIds = workerDtos.SelectMany(s => s.WorkerSkills.Where(i => i.SkillLevelId.HasValue && i.SkillLevelId > 0).Select(i => i.SkillLevelId)).Cast<int>().ToArray();
                var skillDtos = await _skillClientService.Get(skillIds, false);
                var workerSkillLevelDtos = await _skillClientService.GetWorkerSkillLevels(skillLevelIds, false);

                workerDtos.ForEach(workerDto =>
                {
                    workerDto.WorkerSkills.ForEach(workerSkill =>
                    {
                        workerSkill.Skill = skillDtos.FirstOrDefault(s => s.Id == workerSkill.SkillId);
                        workerSkill.SkillLevel = workerSkillLevelDtos.FirstOrDefault(s => s.Id == workerSkill.SkillLevelId);
                    });
                });
            }

            return workerDtos;
        }
    }
}
