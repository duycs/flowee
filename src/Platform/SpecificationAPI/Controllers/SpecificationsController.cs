using AppShareDomain.DTOs.Specification;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using SpecificationApplication.Commands;
using SpecificationApplication.ViewModels;
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
        public async Task<IActionResult> Add([FromBody] CreateSpecificationVM createSpecificationVM)
        {
            var createSpecificationCommand = _mappingService.Map<CreateSpecificationCommand>(createSpecificationVM);
            await _commandDispatcher.Send(createSpecificationCommand);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PathUpdate(int id, [FromBody] UpdateSpecificationVM updateSpecificationVM)
        {
            var updateSpecificationCommand = _mappingService.Map<UpdateSpecificationCommand>(updateSpecificationVM);
            await _commandDispatcher.Send(updateSpecificationCommand);
            return Ok();
        }

        [HttpPost("{id}/build-instruction")]
        public async Task<IActionResult> BuildInstruction(int id)
        {
            var specificationExisting = _repositoryService.Find<Specification>(id, new SpecificationSpecification(true));
            if (specificationExisting is null)
            {
                return BadRequest("The Specification not found");
            }

            _repositoryService.Update(specificationExisting.BuildInstruction());
            var result = _repositoryService.SaveChanges();

            if (!result)
            {
                return StatusCode(500, "The Specification could not update build");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var specificationExisting = _repositoryService.Find<Specification>(id);

            if (specificationExisting is null)
            {
                return BadRequest("The Specification not found");
            }

            _repositoryService.Delete<Specification>(specificationExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool isInclude = true, [FromQuery] int pageNumber = -1, [FromQuery] int pageSize = 0, [FromQuery] string? columnOrders = "", [FromQuery] int[]? ids = null, string? searchValue = "")
        {
            if (pageNumber == -1)
            {
                var specifications = _repositoryService.List<Specification>(ids, new SpecificationSpecification(isInclude)).ToList();
                var specificationDtos = _mappingService.Map<List<SpecificationDto>>(specifications);

                return Ok(specificationDtos);
            }
            else
            {
                var filter = new PaginationFilterOrder(pageNumber, pageSize, columnOrders);
                var specificationSpecification = new SpecificationSpecification(isInclude, searchValue, ids, filter.ColumnOrders.ToColumnOrders());
                var pagedSpecifications = _repositoryService.Find<Specification>(filter.PageNumber, filter.PageSize, specificationSpecification, out int totalRecords).ToList();
                var specificationDtos = _mappingService.Map<List<SpecificationDto>>(pagedSpecifications);
                var pagedReponse = PaginationHelper.CreatePagedReponse<SpecificationDto>(specificationDtos, filter, totalRecords, _uriService, Request.Path.Value ?? "");
                return Ok(pagedReponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude = true)
        {
            var specificationExisting = _repositoryService.Find<Specification>(id, new SpecificationSpecification(isInclude));

            if (specificationExisting is null)
            {
                return NotFound();
            }

            var specificationDto = _mappingService.Map<SpecificationDto>(specificationExisting);
            return Ok(specificationDto);
        }
    }
}