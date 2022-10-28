using AppShareApplication.Services;
using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Services;
using JobApplication.ViewModels;
using JobDomain.AgreegateModels.JobAgreegate;

namespace JobApplication.Services
{
    public class JobService : IJobService
    {
        private readonly ICatalogClientService _catalogClientService;
        private readonly ISkillClientService _skillClientService;
        private readonly IOperationClientService _operationClientService;
        private readonly IWorkerClientService _workerClientService;
        private readonly IStepService _stepService;
        private readonly IMappingService _mappingService;
        private readonly IRepositoryService _repositoryService;

        public JobService(ICatalogClientService catalogClientService, ISkillClientService skillClientService,
            IOperationClientService operationClientService, IWorkerClientService workerClientService,
            IStepService stepService,
            MappingService mappingService, IRepositoryService repositoryService)
        {
            _catalogClientService = catalogClientService;
            _skillClientService = skillClientService;
            _operationClientService = operationClientService;
            _workerClientService = workerClientService;
            _stepService = stepService;
            _mappingService = mappingService;
            _repositoryService = repositoryService;
        }

        public async Task<JobDto> Create(CreateJobVM createJobVM)
        {
            var jobCreated = _repositoryService.Add<Job>(Job.Create(createJobVM.ProductId, createJobVM.Description));
            _repositoryService.SaveChanges();
            var jobDto = _mappingService.Map<JobDto>(jobCreated);
            return jobDto;
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

        public async Task<List<StepDto>> GenerateSteps(int jobId, bool? isInclude)
        {
            var jobExisting = _repositoryService.Find<Job>(jobId);
            if (jobExisting is null)
            {
                throw new Exception("Job not found");
            }

            var stepDtos = new List<StepDto>();
            var specificationDtos = await GetSpecifications(jobExisting.CatalogId);
            if (specificationDtos is null || !specificationDtos.Any())
            {
                throw new Exception("Job do not exist any specifications");
            }

            var operationDtos = specificationDtos.Where(s => s.Operations is not null && s.Operations.Any()).SelectMany(s => s.Operations).ToList();
            var operationIds = operationDtos.Select(o => o.Guid).ToArray();
            if (operationIds.Any())
            {
                var steps = new List<Step>();
                var skillDtos = await _skillClientService.GetSkillsByOperations(operationIds, isInclude);

                if (skillDtos is not null && skillDtos.Any())
                {
                    foreach (var skillDto in skillDtos)
                    {
                        if (skillDto.Operations is not null && skillDto.Operations.Any())
                        {
                            // New step have Job, Skill and Operations of the Skill
                            var step = Step.Create(jobId, skillDto.Id, skillDto.Operations.Select(o => o.Guid).ToList());
                            steps.Add(step);
                        }
                    }

                    // Chain of steps?

                    stepDtos = _mappingService.Map<List<StepDto>>(steps);
                }
            }

            return stepDtos;
        }

        public async Task<List<StepDto>> GenerateUpdateSteps(int jobId)
        {
            var stepDtos = await GenerateSteps(jobId, true);
            if (stepDtos.Any())
            {

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

            var jobExisting = _repositoryService.Find<Job>(jobId);
            return _mappingService.Map<List<StepDto>>(jobExisting.Steps);
        }

        public async Task AutoAssignWorkers(int jobId)
        {
            // Find all skills of job
            var job = _repositoryService.Find<Job>(jobId);
            if (job is null)
            {
                return;
            }

            var jobSkillIds = job.Steps.Select(s => s.SkillId).ToArray();
            if (!jobSkillIds.Any())
            {
                return;
            }

            // Get all active workers with skills is matched
            var activeJobSkillWorkerDtos = await _workerClientService.GetWorkersActive(jobSkillIds, true);
            if (!activeJobSkillWorkerDtos.Any())
            {
                return;
            }

            var workerSkillIds = activeJobSkillWorkerDtos.Select(w => new { w.Id, SkilIds = w.WorkerSkills.Select(ws => ws.SkillId).ToList() }).ToList();

            // Assignee: Worker Skills match to Steps
            foreach (var step in job.Steps)
            {
                var matchedWorkerSkill = workerSkillIds.FirstOrDefault(w => w.SkilIds.Contains(step.SkillId));
                if (matchedWorkerSkill is not null)
                {
                    step.AssignWorker(matchedWorkerSkill.Id);
                }
            }

            // Update steps after assigned
            _repositoryService.Update(job.Steps.ToArray());
            _repositoryService.SaveChanges();
        }

        public async Task Start(int jobId)
        {
            var job = _repositoryService.Find<Job>(jobId);
            if (job is null || job.Steps is null || !job.Steps.Any())
            {
                return;
            }

            // All operations of job
            var operationIds = job.Steps.Where(s => s.OperationIds is not null && s.OperationIds.Any()).SelectMany(s => s.OperationIds).ToArray();

            // Fire event performed operations by humance or auto
            bool isSuccess = await _operationClientService.FireOperations(operationIds);
            if (isSuccess)
            {
                job.StartJob();
                _repositoryService.Update(job);
                _repositoryService.SaveChanges();
            }
        }

        public JobDto? Transformed(int jobId, Guid operationId, out bool isChange)
        {
            isChange = false;

            // Job do not exist
            var job = _repositoryService.Find<Job>(j => j.Id == jobId);
            if (job is null)
            {
                return null;
            }

            // Step need trigger is step have operation do not performed
            // Choose first default step valid
            var stepTrigger = job.Steps.FirstOrDefault(s => s.GetOperationNotPerformed(operationId) is not null);

            // Step do not exist
            if (stepTrigger is null)
            {
                return null;
            }

            // Change step then update job
            var jobTransformed = job.Transformed(stepTrigger.Id, out isChange);
            if (isChange)
            {
                _repositoryService.Update(jobTransformed);
                _repositoryService.SaveChanges();

                return _mappingService.Map<JobDto>(jobTransformed);
            }

            return _mappingService.Map<JobDto>(job);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        private Step FindFirstStep(Job job)
        {
            var step = job.Steps.FirstOrDefault();
            var operationIds = job.Steps.Select(s => s.OperationIds).ToList();

            // What is operation is execute first => step is execute first

            return step;
        }


    }
}
