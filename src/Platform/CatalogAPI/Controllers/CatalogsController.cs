using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using CatalogApplication.Commands;
using CatalogApplication.DTOs;
using CatalogApplication.Services;
using CatalogDomain.AgreegateModels.CatalogAgreegate;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            IRepositoryService repositoryService, IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _catalogService = catalogService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCatalogCommand createCatalogCommand)
        {
            await _commandDispatcher.Send(createCatalogCommand);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUpdate([FromBody] UpdateCatalogCommand updateCatalogCommand)
        {
            await _commandDispatcher.Send(updateCatalogCommand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var catalogExisting = _repositoryService.Find<Catalog>(id);

            if (catalogExisting == null)
            {
                return BadRequest("Catalog not found");
            }

            _repositoryService.Delete<Catalog>(catalogExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var catalogSpecification = new CatalogSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Catalog>(filter.PageNumber, filter.PageSize, catalogSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Catalog>(pagedData, filter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var catalogDto = _catalogService.Get(id, isInclude);
            if (catalogDto == null)
            {
                return NotFound();
            }

            return Ok(catalogDto);
        }
    }
}