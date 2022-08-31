using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;
using AppShareServices.Mappings;
using AppShareServices.Services;
using JobDomain.AgreegateModels.JobAgreegate;

namespace JobApplication.Services
{
    public class JobService : IJobService
    {
        private readonly ICatalogClientService _catalogClientService;
        private readonly ISkillClientService _skillClientService;
        private readonly IMappingService _mappingService;

        public JobService(ICatalogClientService catalogClientService, ISkillClientService skillClientService, IMappingService mappingService)
        {
            _catalogClientService = catalogClientService;
            _skillClientService = skillClientService;
            _mappingService = mappingService;
        }

        public async Task<List<StepDto>> GenerateSteps(int jobId, int productId, bool isInclude)
        {
            var stepDtos = new List<StepDto>();
            var specificationDtos = await GetSpecifications(productId);
            if (specificationDtos is not null && specificationDtos.Any())
            {
                var operationDtos = specificationDtos.Where(s => s is not null && s.Operations.Any()).SelectMany(s => s.Operations).ToList();
                var operationIds = operationDtos.Select(o => o.Guid).ToArray();
                if (operationIds.Any())
                {
                    var steps = new List<Step>();
                    var skillDtos = await _skillClientService.GetSkillsByOperations(operationIds, isInclude);
                    foreach (var skill in skillDtos)
                    {
                        if (skill.Operations is not null && skill.Operations.Any())
                        {
                            var stepOperationIds = skill.Operations.Select(o => o.Guid).ToList();
                            steps.Add(Step.Create(jobId, skill.Id, stepOperationIds));
                        }
                    }
                    stepDtos = _mappingService.Map<List<StepDto>>(steps);
                }
            }

            return stepDtos;
        }

        public async Task<List<SpecificationDto>> GetSpecifications(int productId)
        {
            var specificationDtos = new List<SpecificationDto>();
            var catalogId = productId;
            var catalogDto = await _catalogClientService.Get(catalogId, true);
            if (catalogDto is not null && catalogDto.Addons is not null && catalogDto.Addons.Any())
            {
                var addonSpecifications = catalogDto.Addons.Where(c => c is not null).Select(a => a.Specification).ToList();
                specificationDtos.Add(_mappingService.Map<SpecificationDto>(addonSpecifications));
            }

            return specificationDtos;
        }
    }
}
