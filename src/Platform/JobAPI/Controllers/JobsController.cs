using AppShareDomain.DTOs.Job;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using JobApplication.Commands;
using JobApplication.Services;
using JobApplication.ViewModels;
using JobDomain.AgreegateModels.JobAgreegate;
using Microsoft.AspNetCore.Mvc;

namespace JobAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly IJobService _jobService;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;
        private readonly ICommandDispatcher _commandDispatcher;

        public JobsController(ILogger<JobsController> logger,
            IRepositoryService repositoryService,
            IJobService jobService,
            IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _jobService = jobService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateJobVM createJobVM)
        {
            var result = await _jobService.Create(createJobVM);
            if (result is null)
            {
                return StatusCode(500, "The Job can not add");
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var jobSpecification = new JobSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var jobs = _repositoryService.Find<Job>(filter.PageNumber, filter.PageSize, jobSpecification, out totalRecords).ToList();
            var jobDtos = _mappingService.Map<List<JobDto>>(jobs);
            var pagedReponse = PaginationHelper.CreatePagedReponse<JobDto>(jobDtos, filter, totalRecords, _uriService, Request.Path.Value ?? "");
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var jobExisting = _repositoryService.Find<Job>(id, new JobSpecification(isInclude));

            if (jobExisting is null)
            {
                return NotFound();
            }

            return Ok(jobExisting);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var jobExisting = _repositoryService.Find<Job>(id);

            if (jobExisting is null)
            {
                return BadRequest("The Job not found");
            }

            _repositoryService.Delete<Job>(jobExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet("{id}/generate-steps")]
        public async Task<IActionResult> GenerateSteps(int id)
        {
            var stepDtos = await _jobService.GenerateSteps(id, true);
            if (stepDtos is null)
            {
                return NotFound();
            }

            return Ok(stepDtos);
        }

        [HttpPost("{id}/generate-update-steps")]
        public async Task<IActionResult> GenerateUpdateSteps(int id)
        {
            var stepDtos = await _jobService.GenerateUpdateSteps(id);
            return Ok(stepDtos);
        }

        [HttpPut("{id}/assign-workers")]
        public async Task<IActionResult> AssignWorkers(int id)
        {
            await _jobService.AutoAssignWorkers(id);
            return Ok();
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            await _jobService.Start(id);
            return Ok();
        }

        [HttpPut("{id}/transformed")]
        public async Task<IActionResult> Transformed(int id, int stepId)
        {
            var job = _jobService.Transformed(id, stepId, out bool isChange);
            return Ok(new { Job = job, IsChange = isChange });
        }
    }
}