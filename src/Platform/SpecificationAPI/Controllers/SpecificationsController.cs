using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using SpecificationApplication.Commands;
using SpecificationDomain.AgreegateModels.SpecificationAgreegate;

namespace SpecificationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecificationsController : ControllerBase
    {
        private readonly ILogger<SpecificationsController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;
        private readonly ICommandDispatcher _commandDispatcher;

        public SpecificationsController(ILogger<SpecificationsController> logger, IRepositoryService repositoryService,
            IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSpecificationCommand createSpecificationCommand)
        {
            await _commandDispatcher.Send(createSpecificationCommand);
            return Ok();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PathUpdate(int id, [FromBody] UpdateSpecificationCommand updateSpecificationCommand)
        {
            await _commandDispatcher.Send(updateSpecificationCommand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var specificationExisting = _repositoryService.Find<Specification>(id);

            if (specificationExisting == null)
            {
                return BadRequest("Specification not found");
            }

            _repositoryService.Delete<Specification>(specificationExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var specificationSpecification = new SpecificationSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Specification>(filter.PageNumber, filter.PageSize, specificationSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Specification>(pagedData, filter, totalRecords, _uriService, Request.Path.Value ?? "");
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var specificationExisting = _repositoryService.Find<Specification>(id, new SpecificationSpecification(isInclude));

            if (specificationExisting == null)
            {
                return NotFound();
            }

            return Ok(specificationExisting);
        }
    }
}