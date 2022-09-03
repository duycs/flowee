using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;
using AppShareServices.DataAccess.Repository;
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
        private readonly IRepositoryService _repositoryService;

        public JobService(ICatalogClientService catalogClientService, ISkillClientService skillClientService, IMappingService mappingService, IRepositoryService repositoryService)
        {
            _catalogClientService = catalogClientService;
            _skillClientService = skillClientService;
            _mappingService = mappingService;
            _repositoryService = repositoryService;
        }

        public Task AssignWorkersToJob(int jobId, int[] workerIds)
        {
            throw new NotImplementedException();
        }

        public Task AssignWorkerToStep(int stepId, int workerId)
        {
            // Find skills of workers

            // Matching skill of worker with skill of step

            // Add worker matched to the step

            throw new NotImplementedException();
        }

        public async Task<List<StepDto>> GenerateSteps(int jobId, int productId, bool? isInclude)
        {
            var jobExisting = _repositoryService.Find<Job>(jobId);
            if (jobExisting is null)
            {
                throw new Exception("Job not found");
            }

            var stepDtos = new List<StepDto>();
            var specificationDtos = await GetSpecifications(productId);
            if (specificationDtos is not null && specificationDtos.Any())
            {
                var operationDtos = specificationDtos.Where(s => s is not null && s.Operations.Any()).SelectMany(s => s.Operations).ToList();
                var operationIds = operationDtos.Select(o => o.Guid).ToArray();
                if (operationIds.Any())
                {
                    var skillDtos = await _skillClientService.GetSkillsByOperations(operationIds, isInclude);
                    foreach (var skillDto in skillDtos)
                    {
                        if (skillDto.Operations is not null && skillDto.Operations.Any())
                        {
                            // New step have Job, Skill and Operations of the Skill
                            var stepDto = new StepDto();
                            stepDto.JobId = jobId;
                            stepDto.SkillId = skillDto.Id;
                            stepDto.Job = _mappingService.Map<JobDto>(jobExisting);
                            stepDto.OperationIds = skillDto.Operations.Select(o => o.Guid).ToList();
                            stepDto.Operations = skillDto.Operations;

                            stepDtos.Add(stepDto);
                        }
                    }
                }
            }

            return stepDtos;
        }

        public async Task<List<SpecificationDto>> GetSpecifications(int productId)
        {
            var specificationDtos = new List<SpecificationDto>();
            var catalogId = productId;
            var catalogDto = await _catalogClientService.Get(catalogId, true);

            // catalog specification
            specificationDtos.Add(catalogDto.Specification);

            // addon specifications
            if (catalogDto is not null && catalogDto.Addons is not null && catalogDto.Addons.Any())
            {
                var addonSpecifications = catalogDto.Addons.Where(c => c is not null).Select(a => a.Specification).ToList();
                specificationDtos.Add(_mappingService.Map<SpecificationDto>(addonSpecifications));
            }

            return specificationDtos;
        }

        public Task SetLastSteps(int[] lastSteps)
        {
            throw new NotImplementedException();
        }

        public Task SetNextSteps(int[] nextStepIds)
        {
            throw new NotImplementedException();
        }

        public Task UpdateJobStatus(EnumerationDto jobStatus)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSteps(int jobId, int productId)
        {
            var stepDtos = await GenerateSteps(jobId, productId, true);
            if (!stepDtos.Any())
            {
                return;
            }

            // Remove all existing steps by job
            var stepsByJobExisting = _repositoryService.ListAsQueryable<Step>(s => s.JobId == jobId).ToArray();
            if (stepsByJobExisting.Any())
            {
                _repositoryService.Delete(stepsByJobExisting);
            }

            // Add steps for job
            foreach (var stepDto in stepDtos)
            {
                var step = Step.Create(stepDto.JobId, stepDto.SkillId, stepDto.OperationIds?.ToList());
                _repositoryService.Add(step);
            }

            _repositoryService.SaveChanges();
        }
    }
}
