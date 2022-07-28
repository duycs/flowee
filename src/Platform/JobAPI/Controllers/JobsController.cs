using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
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
            _repositoryService.Add<Job>(Job.Create(createJobVM.ProductId, createJobVM.Description));
            var result = _repositoryService.SaveChanges();
            if (!result)
            {
                return StatusCode(500, "The Job can not add");
            }

            return Ok();
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var jobSpecification = new JobSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Job>(filter.PageNumber, filter.PageSize, jobSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Job>(pagedData, filter, totalRecords, _uriService, Request.Path.Value ?? "");
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

        [HttpGet("generate-step-from-product")]
        public async Task<IActionResult> GenerateStepFromProduct([FromQuery] int produtId)
        {
            var steps = await _jobService.GenerateStepFromProduct(produtId);
            return Ok(steps);
        }
    }
}