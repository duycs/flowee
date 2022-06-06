using AppShareServices.Models;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace WorkerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;
        private readonly IWorkerRepository _workerRepository;
        private readonly IUriService _uriService;

        public WorkerController(ILogger<WorkerController> logger,
            IUriService uriService,
            IWorkerRepository workerRepository)
        {
            _logger = logger;
            _uriService = uriService;
            _workerRepository = workerRepository;
        }

        [HttpGet(Name = "Workers")]
        public async Task<IActionResult> Get([FromQuery] PaginationFilter filter, string searchValue, List<ColumnOrder> columnOrders)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = 0;
            Expression<Func<Worker, bool>> criteria = w => w.Id != null;
            var workerSpecification = new WorkerSpecification(criteria, searchValue, columnOrders);
            var pagedData = _workerRepository.Find<Worker>(validFilter.PageNumber, validFilter.PageSize, workerSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Worker>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
    }
}