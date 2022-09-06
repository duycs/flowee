using AppShareApplication.Services;
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
        private readonly IOperationClientService _operationClientService;
        private readonly IStepService _stepService;
        private readonly IMappingService _mappingService;
        private readonly IRepositoryService _repositoryService;

        public JobService(ICatalogClientService catalogClientService, ISkillClientService skillClientService, IOperationClientService operationClientService,
            IStepService stepService,
            MappingService mappingService, IRepositoryService repositoryService)
        {
            _catalogClientService = catalogClientService;
            _skillClientService = skillClientService;
            _operationClientService = operationClientService;
            _stepService = stepService;
            _mappingService = mappingService;
            _repositoryService = repositoryService;
        }

        public async Task AutoAssignWorkersToJob(int jobId)
        {
            // Find all skills of job

            // Get all active workers

            // Assignee: Worker Skills match to Steps

            throw new NotImplementedException();
        }

        /// <summary>
        /// Find a active worker match to Step
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task AutoAssignWorkerToStep(int jobId, int stepId)
        {
            // Assignee: Worker Skill match to Step

            throw new NotImplementedException();
        }

        /// <summary>
        /// Start job: fire performed operations
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public async Task Start(int jobId)
        {
            var steps = _repositoryService.List<Step>(s => s.JobId == jobId);
            if (steps.Any())
            {
                foreach (var step in steps)
                {
                    // Trigger start performed operations
                    if (step.OperationIds is not null && step.OperationIds.Any())
                    {
                        await _operationClientService.PerformedOperations(step.OperationIds.ToArray());
                    }
                }
            }
        }


        public async Task<List<StepDto>> GenerateSteps(int jobId, bool? isInclude)
        {
            var jobExisting = _repositoryService.Find<Job>(jobId);
            if (jobExisting is null)
            {
                throw new Exception("Job not found");
            }

            var stepDtos = new List<StepDto>();
            var specificationDtos = await GetSpecifications(jobExisting.CatalogId);
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

        public async Task<List<SpecificationDto>> GetSpecifications(int catalogId)
        {
            var specificationDtos = new List<SpecificationDto>();
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

        public async Task UpdateSteps(int jobId)
        {
            var stepDtos = await GenerateSteps(jobId, true);
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

        public void Transformed(int jobId, int stepId)
        {
            var jobExisting = _repositoryService.Find<Job>(j => j.Id == jobId);
            if(jobExisting is not null)
            {
                // Change step then update
                var job = jobExisting.Transformed(stepId, out bool isChange);
                if (isChange)
                {
                    _repositoryService.Update(job);
                    _repositoryService.SaveChanges();
                }
            }
        }
    }
}
