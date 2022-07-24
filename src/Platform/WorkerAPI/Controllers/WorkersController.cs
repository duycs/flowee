using AppShareDomain.DTOs.Worker;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using WorkerApplication.Commands;
using WorkerApplication.ViewModels;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

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

        /// <summary>
        /// Add worker and relation Roles, Groups, Skills
        /// </summary>
        /// <param name="createWorkerVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateWorkerVM createWorkerVM)
        {
            var createWorkerCommand = _mappingService.Map<CreateWorkerCommand>(createWorkerVM);
            await _commandDispatcher.Send(createWorkerCommand);

            return Ok();
        }


        /// <summary>
        /// Add more Groups, Roles, Skills if have
        /// </summary>
        /// <param name="pathUpdateWorkerVM"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PathUpdateWorkerVM pathUpdateWorkerVM)
        {
            var updateWorkerCommand = _mappingService.Map<UpdateWorkerCommand>(pathUpdateWorkerVM);
            await _commandDispatcher.Send(updateWorkerCommand);

            return Ok();
        }

        /// <summary>
        /// Delete worker and relations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workerExisting = _repositoryService.Find<Worker>(id);
            if (workerExisting is null)
            {
                return NotFound();
            }

            _repositoryService.Delete(workerExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var workerSpecification = new WorkerSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Worker>(filter.PageNumber, filter.PageSize, workerSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Worker>(pagedData, filter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var workerExisting = _repositoryService.Find<Worker>(id, new WorkerSpecification(isInclude));
            if (workerExisting is null)
            {
                return NotFound();
            }

            var workerDto = _mappingService.Map<WorkerDto>(workerExisting);
            return Ok(workerDto);
        }

    }
}