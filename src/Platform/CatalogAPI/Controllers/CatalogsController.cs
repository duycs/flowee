using AppShareDomain.DTOs.Catalog;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using CatalogApplication.Commands;
using CatalogApplication.Services;
using CatalogApplication.ViewModels;
using CatalogDomain.AgreegateModels.CatalogAgreegate;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogsController : ControllerBase
    {
        private readonly ILogger<CatalogsController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;
        private readonly ICommandDispatcher _commandDispatcher;

        public CatalogsController(ILogger<CatalogsController> logger, ICatalogService catalogService,
            IRepositoryService repositoryService, IMappingService mappingService,
            IUriService uriService, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _catalogService = catalogService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCatalogVM createCatalogVM)
        {
            var createCatalogCommand = _mappingService.Map<CreateCatalogCommand>(createCatalogVM);
            await _commandDispatcher.Send(createCatalogCommand);
            return Ok();
        }

        [HttpPost("{id}/build-description")]
        public async Task<IActionResult> BuildCatalogDescription(int id)
        {
            await _catalogService.BuildCatalogDescription(id);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUpdate([FromBody] UpdateCatalogVM updateCatalogVM)
        {
            var updateCatalogCommand = _mappingService.Map<UpdateCatalogCommand>(updateCatalogVM);
            await _commandDispatcher.Send(updateCatalogCommand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var catalogExisting = _repositoryService.Find<Catalog>(id);

            if (catalogExisting is null)
            {
                return BadRequest("Catalog not found");
            }

            _repositoryService.Delete<Catalog>(catalogExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(bool isInclude = true, [FromQuery] int pageNumber = -1, [FromQuery] int pageSize = 0, [FromQuery] string? columnOrders = "", [FromQuery] int[]? ids = null, string? searchValue = "")
        {
            if (pageNumber == -1)
            {
                var catalogDtos = await _catalogService.Find(ids, isInclude);
                return Ok(catalogDtos);
            }
            else
            {
                var filter = new PaginationFilterOrder(pageNumber, pageSize, columnOrders);
                var pagedData = _catalogService.Find(filter.PageNumber, filter.PageSize, filter.ColumnOrders, searchValue, isInclude, out int totalRecords);
                var pagedReponse = PaginationHelper.CreatePagedReponse<CatalogDto>(pagedData, filter, totalRecords, _uriService, Request.Path.Value);
                return Ok(pagedReponse);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude = true)
        {
            var catalogDto = await _catalogService.Find(id, isInclude);
            if (catalogDto is null)
            {
                return NotFound();
            }

            return Ok(catalogDto);
        }
    }
}