using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using CatalogApplication.Commands;
using CatalogApplication.DTOs;
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
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogsController(ILogger<CatalogsController> logger, IRepositoryService repositoryService,
            IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
            _httpClientFactory = httpClientFactory;
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
            var catalogExisting = _repositoryService.Find<Catalog>(id, new CatalogSpecification(isInclude));

            if (catalogExisting == null)
            {
                return NotFound();
            }

            var catalogDto = _mappingService.Map<CatalogDto>(catalogExisting);

            if (isInclude)
            {
                if (catalogExisting.SpecificationId != null)
                {
                    var httpClient = _httpClientFactory.CreateClient("Specification");
                    var baseObjectUrl = $"{catalogExisting.SpecificationId}";
                    var httpResponseStandarSpecificationMessage = await httpClient.GetAsync($"/{baseObjectUrl}");

                    if (httpResponseStandarSpecificationMessage.IsSuccessStatusCode)
                    {
                        using var contentStream = await httpResponseStandarSpecificationMessage.Content.ReadAsStreamAsync();
                        var specificationDto = await JsonSerializer.DeserializeAsync<SpecificationDto>(contentStream);
                        if (specificationDto != null)
                        {
                            catalogDto.Specification = specificationDto;
                        }
                    }

                    // Find list specifications of addon
                    if (catalogExisting.Addons != null && catalogExisting.Addons.Any())
                    {
                        var specificationAddonIds = catalogExisting.Addons.Select(x => x.SpecificationId).ToList();

                        var httpResponseAddonSpecificationMessage = await httpClient
                            .GetAsync($"/{baseObjectUrl}/addons/{string.Join(",", specificationAddonIds)}");
                        if (httpResponseAddonSpecificationMessage.IsSuccessStatusCode)
                        {
                            using var contentStream = await httpResponseAddonSpecificationMessage.Content.ReadAsStreamAsync();
                            var specificationDtos = await JsonSerializer.DeserializeAsync<List<SpecificationDto>>(contentStream);

                            if (specificationDtos != null && specificationDtos.Any())
                            {
                                foreach (var addon in catalogDto.Addons)
                                {
                                    var specificationDto = specificationDtos.FirstOrDefault(c => c.Id == addon.Specification.Id);
                                    if (specificationDto != null)
                                    {
                                        addon.Specification = specificationDto;
                                    }
                                }
                            }
                        }
                    }
                }

                return Ok(catalogDto);
            }

            return Ok(catalogExisting);
        }
    }
}