using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;
using AppShareServices.Mappings;
using AppShareServices.Services;
using JobDomain.AgreegateModels.JobAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<StepDto>> GenerateStepFromProduct(int productId)
        {
            var steps = new List<Step>();
            int orderNumber = 0;
            var catalogId = productId;
            var catalogDto = await _catalogClientService.Get(catalogId, true);
            var specificationDtos = new List<SpecificationDto>();

            if (catalogDto is not null)
            {
                //var matrixSkills = await _skillClientService.GetMatrixSkills((int)specificationSkill.SkillId, null, specification.Id, true);

                var catalogStep = MappingSpecificationSkillToStep(catalogDto.Specification, ref orderNumber);
                steps.AddRange(catalogStep);

                if (catalogDto.Addons is not null && catalogDto.Addons.Any())
                {
                    foreach (var addon in catalogDto.Addons)
                    {
                        var addOnSteps = MappingSpecificationSkillToStep(addon.Specification, ref orderNumber);
                        steps.AddRange(addOnSteps);

                        specificationDtos.Add(addon.Specification);
                    }
                }

                specificationDtos.Add(catalogDto.Specification);
            }

            var stepDtos = _mappingService.Map<List<StepDto>>(steps);

            stepDtos.ForEach(stepDto =>
            {
                stepDto.Specification = specificationDtos.FirstOrDefault(s => s.Id == stepDto.SpecificationId);
            });

            return stepDtos;
        }

        private List<Step> MappingSpecificationSkillToStep(SpecificationDto specification, ref int orderNumber)
        {
            var steps = new List<Step>();
            if (specification is not null && specification.SpecificationSkills is not null && specification.SpecificationSkills.Any())
            {
                foreach (var specificationSkill in specification.SpecificationSkills)
                {
                    orderNumber++;
                    steps.Add(Step.Create(specification.Id, specificationSkill.SkillId, specification.Instruction, orderNumber));
                }
            }

            return steps;
        }
    }
}
