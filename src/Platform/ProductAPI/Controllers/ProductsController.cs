using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.Commands;
using ProductDomain.AgreegateModels.ProductAgreegate;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;
        private readonly ICommandDispatcher _commandDispatcher;

        public ProductsController(ILogger<ProductsController> logger, IRepositoryService repositoryService,
            IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductCommand createProductCommand)
        {
            await _commandDispatcher.Send(createProductCommand);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUpdate([FromBody] UpdateProductCommand updateProductCommand)
        {
            await _commandDispatcher.Send(updateProductCommand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var productExisting = _repositoryService.Find<Product>(id);

            if (productExisting == null)
            {
                return BadRequest("Product not found");
            }

            _repositoryService.Delete<Product>(productExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var productSpecification = new ProductSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Product>(filter.PageNumber, filter.PageSize, productSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Product>(pagedData, filter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var productExisting = _repositoryService.Find<Product>(id, new ProductSpecification(isInclude));

            if (productExisting == null)
            {
                return NotFound();
            }

            return Ok(productExisting);
        }

    }
}