using AppShareDomain.DTOs.Skill;
using AppShareDomain.DTOs.Specification;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using AppShareServices.Services;
using SpecificationApplication.Commands;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace SpecificationApplication.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IRepositoryService _repositoryService;
        private readonly ISkillClientService _skillClientService;
        private readonly IMappingService _mappingService;
        private readonly ICommandDispatcher _commandDispatcher;

        public SpecificationService(IRepositoryService repositoryService, ISkillClientService skillClientService,
            IMappingService mappingService, ICommandDispatcher commandDispatcher)
        {
            _repositoryService = repositoryService;
            _skillClientService = skillClientService;
            _mappingService = mappingService;
            _commandDispatcher = commandDispatcher;
        }

        public async Task<SpecificationDto?> Find(int id, bool isInclude)
        {
            var specificationExisting = _repositoryService.Find<Specification>(id, new SpecificationSpecification(isInclude));
            var specificationDto = _mappingService.Map<SpecificationDto>(specificationExisting);
            if (specificationDto is not null && specificationDto.SpecificationSkills is not null && specificationDto.SpecificationSkills.Any())
            {
                var skillIds = specificationDto.SpecificationSkills.Where(s => s.SkillId.HasValue && s.SkillId > 0).Select(s => s.SkillId).Cast<int>().ToArray();
                var skillLevelIds = specificationDto.SpecificationSkills.Where(s => s.SkillLevelId.HasValue && s.SkillLevelId > 0).Select(s => s.SkillLevelId).Cast<int>().ToArray();
                var skillDtos = await _skillClientService.Get(skillIds, false);
                var specificationSkillLevelDtos = await _skillClientService.GetSpecificationSkillLevels(skillLevelIds, false);

                specificationDto.SpecificationSkills.ForEach(specificationSkill =>
                {
                    specificationSkill.Skill = skillDtos.FirstOrDefault(s => s.Id == specificationSkill.SkillId);
                    specificationSkill.SkillLevel = specificationSkillLevelDtos.FirstOrDefault(s => s.Id == specificationSkill.SkillLevelId);
                });

                var operations = specificationExisting.GetOperations();
                if (operations is not null && operations.Any())
                {
                    specificationDto.Operations = _mappingService.Map<List<OperationDto>>(operations);
                }
            }

            return specificationDto;
        }

        public async Task<IEnumerable<SpecificationDto>?> Find(int[] ids, bool isInclude)
        {
            var specificationsExisting = _repositoryService.List<Specification>(ids, new SpecificationSpecification(isInclude));
            var specificationDtos = _mappingService.Map<List<SpecificationDto>>(specificationsExisting);
            if (isInclude)
            {
                specificationDtos = await FindInclude(specificationDtos);
            }

            return specificationDtos;
        }

        public List<SpecificationDto> Find(int pageNumber, int pageSize, string columnOrders, int[] ids, string searchValue, bool isInclude, out int totalRecords)
        {
            var specificationSpecification = new SpecificationSpecification(isInclude, searchValue, ids, columnOrders.ToColumnOrders());
            var pagedWorkers = _repositoryService.Find<Specification>(pageNumber, pageSize, specificationSpecification, out totalRecords).ToList();
            var workerDtos = _mappingService.Map<List<SpecificationDto>>(pagedWorkers);
            return FindInclude(workerDtos).Result;
        }

        public async Task<List<SpecificationDto>> FindInclude(List<SpecificationDto> specificationDtos)
        {
            if (specificationDtos.Any())
            {
                var skillIds = specificationDtos.SelectMany(s => s.SpecificationSkills.Where(i => i.SkillId.HasValue && i.SkillId > 0).Select(i => i.SkillId)).Cast<int>().ToArray();
                var skillLevelIds = specificationDtos.SelectMany(s => s.SpecificationSkills.Where(i => i.SkillLevelId.HasValue && i.SkillLevelId > 0).Select(i => i.SkillLevelId)).Cast<int>().ToArray();
                var skillDtos = await _skillClientService.Get(skillIds, false);
                var speccificationSkillLevelDtos = await _skillClientService.GetWorkerSkillLevels(skillLevelIds, false);

                if (speccificationSkillLevelDtos is not null && speccificationSkillLevelDtos.Any())
                {
                    specificationDtos.ForEach(specificationDto =>
                    {
                        specificationDto.SpecificationSkills.ForEach(specificationSkill =>
                        {
                            specificationSkill.Skill = skillDtos.FirstOrDefault(s => s.Id == specificationSkill.SkillId);
                            specificationSkill.SkillLevel = speccificationSkillLevelDtos.FirstOrDefault(s => s.Id == specificationSkill.SkillLevelId);
                        });
                    });
                }
            }

            return specificationDtos;
        }

        public List<OperationDto>? GetOperations(int specificationId, bool isInclude)
        {
            var operationDtos = new List<OperationDto>();
            var specificationExisting = _repositoryService.Find<Specification>(specificationId, new SpecificationSpecification(isInclude));
            var operations = specificationExisting.GetOperations();
            if (operations is not null && operations.Any())
            {
                operationDtos = _mappingService.Map<List<OperationDto>>(operations);
            }

            return operationDtos;
        }

        public async Task<List<SkillDto>?> GetSkills(int specificationId, bool isInclude)
        {
            var operations = GetOperations(specificationId, isInclude);
            if (operations is null || !operations.Any())
            {
                return new List<SkillDto>();
            }

            var operationIds = operations.Select(o => o.Guid).ToArray();
            var skills = await _skillClientService.GetSkillsByOperations(operationIds, isInclude);
            return skills.ToList();
        }
    }
}
