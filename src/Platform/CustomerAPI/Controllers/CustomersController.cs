using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using CustomerApplication.Commands;
using CustomerApplication.ViewModels;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using Microsoft.AspNetCore.Mvc;
using WorkerDomain.AgreegateModels.WorkerAgreegate;

namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;
        private readonly ICommandDispatcher _commandDispatcher;

        public CustomersController(ILogger<CustomersController> logger, IRepositoryService repositoryService,
            IMappingService mappingService, IUriService uriService, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _repositoryService = repositoryService;
            _mappingService = mappingService;
            _uriService = uriService;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerVM createCustomerVM)
        {
            var createCustomerCommand = _mappingService.Map<CreateCustomerCommand>(createCustomerVM);
            await _commandDispatcher.Send(createCustomerCommand);
            return Ok();
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PathUpdate(int id, [FromBody] UpdateCustomerVM updateCustomerVM)
        {
            var updateCustomerCommand = _mappingService.Map<UpdateCustomerCommand>(updateCustomerVM);
            await _commandDispatcher.Send(updateCustomerCommand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var customerExisting = _repositoryService.Find<Customer>(id);

            if (customerExisting is null)
            {
                return BadRequest("Customer not found");
            }

            _repositoryService.Delete<Customer>(customerExisting);
            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationFilterOrder filter, string? searchValue, bool isInclude)
        {
            int totalRecords;
            var customerSpecification = new CustomerSpecification(isInclude, searchValue, filter.ColumnOrders.ToColumnOrders());
            var pagedData = _repositoryService.Find<Customer>(filter.PageNumber, filter.PageSize, customerSpecification, out totalRecords).ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Customer>(pagedData, filter, totalRecords, _uriService, Request.Path.Value ?? "");
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool isInclude)
        {
            var customerExisting = _repositoryService.Find<Customer>(id, new CustomerSpecification(isInclude));

            if (customerExisting is null)
            {
                return NotFound();
            }

            return Ok(customerExisting);
        }

    }
}