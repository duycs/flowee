using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Product;
using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.Commands;
using ProductApplication.Services;
using ProductApplication.ViewModels;
using ProductDomain.AgreegateModels.ProductAgreegate;
using System.Text.Json;
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
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IRepositoryService repositoryService,
            IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher,
            IProductService productService)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateProductVM createProductVM)
        {
            var createProductCommand = _mappingService.Map<CreateProductCommand>(createProductVM);
            await _commandDispatcher.Send(createProductCommand);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> PatchUpdate([FromBody] UpdateProductVM updateProductVM)
        {
            var updateProductCommand = _mappingService.Map<UpdateProductCommand>(updateProductVM);
            await _commandDispatcher.Send(updateProductCommand);
            return Ok();
        }

        [HttpPost("{id}/build-instruction")]
        public async Task<IActionResult> BuildInstruction(int id)
        {
            await _productService.BuildInstruction(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var productExisting = _repositoryService.Find<Product>(id);

            if (productExisting is null)
            {
                return BadRequest("Product not found");
            }

            _repositoryService.Delete<Product>(productExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        // TODO: System.Net.Http.HttpRequestException: No connection could be made because the target machine actively refused it.
        [HttpGet]
        public async Task<IActionResult> Get(bool isInclude = true, [FromQuery] int pageNumber = -1, [FromQuery] int pageSize = 0, [FromQuery] string? columnOrders = "", [FromQuery] int[]? ids = null, string? searchValue = "")
        {
            if (pageNumber == -1)
            {
                var productDtos = await _productService.Find(ids, isInclude);
                return Ok(productDtos);
            }
            else
            {
                var filter = new PaginationFilterOrder(pageNumber, pageSize, columnOrders);
                var pagedData = _productService.Find(filter.PageNumber, filter.PageSize, filter.ColumnOrders, searchValue, isInclude, out int totalRecords);
                var pagedReponse = PaginationHelper.CreatePagedReponse<ProductDto>(pagedData, filter, totalRecords, _uriService, Request.Path.Value);
                return Ok(pagedReponse);
            }
        }

        /// <summary>
        /// Get all relation data if include is true
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isInclude"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var productDto = await _productService.Find(id, isInclude);
            return Ok(productDto);
        }
    }
}