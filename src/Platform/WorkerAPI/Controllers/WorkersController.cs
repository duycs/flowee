using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Models;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using WorkerApplication.ViewModels;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerDomain.Commands;

namespace WorkerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly ILogger<WorkersController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;

        public WorkersController(ILogger<WorkersController> logger,
            IMappingService mappingService,
            IUriService uriService,
            IRepositoryService repositoryService,
            ICommandDispatcher commandDispatcher
            )
        {
            _logger = logger;
            _uriService = uriService;
            _mappingService = mappingService;
            _repositoryService = repositoryService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWorkerVM createWorkerVM)
        {
            var createWorkerCommand = _mappingService.Map<CreateWorkerCommand>(createWorkerVM);
            await _commandDispatcher.Send(createWorkerCommand);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Worker worker)
        {
            _repositoryService.Update(worker);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workerExisting = _repositoryService.Find<Worker>(id);
            if (workerExisting == null)
            {
                return NotFound();
            }

            _repositoryService.Delete(workerExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue)
        {
            var route = Request.Path.Value;
            int totalRecords;
            bool isInclude = true;
            var workerSpecification = new WorkerSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Worker>(filter.PageNumber, filter.PageSize, workerSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Worker>(pagedData, filter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var worker = _repositoryService.Find<Worker>(id);
            return Ok(worker);
        }

    }
}