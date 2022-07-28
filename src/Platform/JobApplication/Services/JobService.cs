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

        public JobService(ICatalogClientService catalogClientService, ISkillClientService skillClientService)
        {
            _catalogClientService = catalogClientService;
            _skillClientService = skillClientService;
        }

        public async Task<List<Step>> GenerateStepFromProduct(int productId)
        {
            var steps = new List<Step>();
            int orderNumber = 1;
            var catalogId = productId;
            var catalogDto = await _catalogClientService.Get(catalogId, true);

            if (catalogDto is not null)
            {
                if (catalogDto.Specification is not null && catalogDto.Specification.SpecificationSkills is not null && catalogDto.Specification.SpecificationSkills.Any())
                {
                    foreach (var skill in catalogDto.Specification.SpecificationSkills)
                    {
                        var matrixSkills = await _skillClientService.GetMatrixSkills((int)skill.SkillId, null, catalogDto.Specification.Id, true);
                        var catalogStep = Step.Create(catalogDto.Specification.Id, skill.SkillId, catalogDto.Specification.Instruction, orderNumber);

                        // TODO: matrixSkill -> Step?
                        var firstMatchingMatrixSkill = matrixSkills.FirstOrDefault();
                        catalogStep.WorkerSkillLevelId = firstMatchingMatrixSkill.WorkerSkillLevel.Id;
                        catalogStep.SpecificationSkillLevelId = firstMatchingMatrixSkill.SpecificationSkillLevel.Id;

                        steps.Add(catalogStep);
                    }
                }

                if (catalogDto.Addons is not null && catalogDto.Addons.Any())
                {
                    foreach (var addon in catalogDto.Addons)
                    {
                        if (addon.Specification is not null && addon.Specification.SpecificationSkills is not null && addon.Specification.SpecificationSkills.Any())
                        {
                            foreach (var skill in catalogDto.Specification.SpecificationSkills)
                            {
                                orderNumber++;
                                var addOnStep = Step.Create(addon.Specification.Id, skill.SkillId, addon.Specification.Instruction, orderNumber);
                                steps.Add(addOnStep);
                            }
                        }
                    }
                }
            }

            return steps;
        }
    }
}
