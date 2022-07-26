using AppShareDomain.DTOs.Worker;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using WorkerApplication.Commands;
using WorkerApplication.Services;
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
        private readonly IWorkerService _workerService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;

        public WorkersController(ILogger<WorkersController> logger,
            IMappingService mappingService,
            IUriService uriService,
            IRepositoryService repositoryService,
            IWorkerService workerService,
            ICommandDispatcher commandDispatcher
            )
        {
            _logger = logger;
            _uriService = uriService;
            _mappingService = mappingService;
            _repositoryService = repositoryService;
            _workerService = workerService;
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

        // TODO: ids does not require?
        [HttpGet]
        public async Task<IActionResult> Get(bool isInclude = true, [FromQuery] int pageNumber = -1, [FromQuery] int pageSize = 0, [FromQuery] string? columnOrders = "", [FromQuery] int[]? ids = null, string? searchValue = "")
        {
            if (pageNumber == -1)
            {
                var workerDtos = await _workerService.Find(ids, isInclude);
                return Ok(workerDtos);
            }
            else
            {
                var filter = new PaginationFilterOrder(pageNumber, pageSize, columnOrders);
                var pagedData = _workerService.Find(filter.PageNumber, filter.PageSize, filter.ColumnOrders, searchValue, isInclude, out int totalRecords);
                var pagedReponse = PaginationHelper.CreatePagedReponse<WorkerDto>(pagedData, filter, totalRecords, _uriService, Request.Path.Value);
                return Ok(pagedReponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var workerDto = await _workerService.Find(id, isInclude);
            if (workerDto is null)
            {
                return NotFound();
            }

            return Ok(workerDto);
        }

    }
}