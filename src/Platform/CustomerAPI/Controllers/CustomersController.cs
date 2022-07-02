using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using CustomerApplication.Commands;
using CustomerApplication.ViewModels;
using CustomerDomain.AgreegateModels.CustomerAgreegate;
using Microsoft.AspNetCore.Mvc;

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
    }
}