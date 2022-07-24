using AppShareDomain.DTOs.Worker;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (workerDto is not null)
            {
                if (workerDto.WorkerSkills is not null && workerDto.WorkerSkills.Any())
                {
                    var skillIds = workerDto.WorkerSkills.Where(s => s.SkillId.HasValue && s.SkillId > 0).Select(s => s.SkillId).Cast<int>().ToArray();
                    var skillLevelIds = workerDto.WorkerSkills.Where(s => s.SkillLevelId.HasValue && s.SkillLevelId > 0).Select(s => s.SkillLevelId).Cast<int>().ToArray();
                    var skillDtos = await _skillClientService.Get(skillIds, false);
                    var skillLevelDtos = await _skillClientService.GetSkillLevels(skillLevelIds, false);

                    workerDto.WorkerSkills.ForEach(workerSkill =>
                    {
                        workerSkill.Skill = skillDtos.FirstOrDefault(s => s.Id == workerSkill.SkillId);
                        workerSkill.SkillLevel = skillLevelDtos.FirstOrDefault(s => s.Id == workerSkill.SkillLevelId);
                    });
                }
            }

            return workerDto;
        }

        public async Task<IEnumerable<WorkerDto>?> Find(int[] ids, bool isInclude)
        {
            var workerDtos = new List<WorkerDto>();
            var workersExisting = _repositoryService.List<Worker>(ids, new WorkerSpecification(isInclude));


            return workerDtos;
        }

        public List<WorkerDto> Find(int pageNumber, int pageSize, string columnOrders, string searchValue, bool isInclude, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public Task<List<WorkerDto>> FindInclude(List<WorkerDto> workerDtos)
        {
            throw new NotImplementedException();
        }
    }
}
