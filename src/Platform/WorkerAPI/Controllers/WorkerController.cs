using AppShareServices.DataAccess.Repository;
using AppShareServices.Models;
using AppShareServices.Pagging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WorkerApplication.DTOs;
using WorkerApplication.ViewModels;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private readonly IRepositoryService<Worker> _workerRepository;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public WorkerController(ILogger<WorkerController> logger,
            IUriService uriService,
            IRepositoryService<Worker> workerRepository,
            IMapper mapper)
        {
            _logger = logger;
            _uriService = uriService;
            _workerRepository = workerRepository;
            _mapper = mapper;
        }

        [HttpPost("workers")]
        public async Task<IActionResult> Add([FromBody] WorkerVM workerVm)
        {
            var worker = _mapper.Map<Worker>(workerVm);
            var workerAdded = _workerRepository.Add(worker);
            _workerRepository.SaveChanges();
            return Created(string.Format("workers\\{0}", workerAdded.Id), workerAdded);
        }

        [HttpPut("workers")]
        public async Task<IActionResult> Update([FromBody] Worker worker)
        {
            var workerUpdated = _workerRepository.Update(worker);
            return Ok(workerUpdated);
        }

        [HttpDelete("workers/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workerExisting = _workerRepository.Find<Worker>(id);
            if (workerExisting == null)
            {
                return NotFound();
            }

            var result = _workerRepository.Delete(workerExisting);
            if (!result)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet("workers")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, string searchValue, List<ColumnOrder> columnOrders)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = 0;
            Expression<Func<Worker, bool>> criteria = w => true;
            var workerSpecification = new WorkerSpecification(criteria, searchValue, columnOrders);
            var pagedData = _workerRepository.Find<Worker>(validFilter.PageNumber, validFilter.PageSize, workerSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Worker>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        [HttpGet("workers/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var worker = _workerRepository.Find<Worker>(id);
            return Ok(worker);
        }

    }
}