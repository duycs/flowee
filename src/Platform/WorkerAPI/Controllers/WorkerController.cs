using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Models;
using AppShareServices.Pagging;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WorkerApplication.Services;
using WorkerApplication.ViewModels;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using WorkerDomain.Commands;

namespace WorkerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IWorkerManager _workerManager;

        public WorkerController(ILogger<WorkerController> logger,
            IUriService uriService,
            IRepositoryService repositoryService,
            ICommandDispatcher commandDispatcher,
            IWorkerManager workerManager,
            IMapper mapper)
        {
            _logger = logger;
            _uriService = uriService;
            _repositoryService = repositoryService;
            _commandDispatcher = commandDispatcher;
            _workerManager = workerManager;
            _mapper = mapper;
        }

        [HttpPost("workers")]
        public async Task<IActionResult> Add([FromBody] CreateWorkerVM createWorkerVM)
        {
            await _workerManager.AddWorkerAsync(createWorkerVM);
            return Ok();
        }

        [HttpPut("workers")]
        public async Task<IActionResult> Update([FromBody] Worker worker)
        {
            var workerUpdated = _repositoryService.Update(worker);
            return Ok(workerUpdated);
        }

        [HttpDelete("workers/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workerExisting = _repositoryService.Find<Worker>(id);
            if (workerExisting == null)
            {
                return NotFound();
            }

            var result = _repositoryService.Delete(workerExisting);
            if (!result)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet("workers")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, string? searchValue, List<ColumnOrder>? columnOrders)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = 0;
            Expression<Func<Worker, bool>> criteria = w => true;
            var workerSpecification = new WorkerSpecification(criteria, false, searchValue, columnOrders);
            var pagedData = _repositoryService.Find<Worker>(validFilter.PageNumber, validFilter.PageSize, workerSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Worker>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("workers/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var worker = _repositoryService.Find<Worker>(id);
            return Ok(worker);
        }

    }
}