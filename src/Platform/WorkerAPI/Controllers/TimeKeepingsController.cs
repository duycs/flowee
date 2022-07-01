using AppShareServices.Commands;
using AppShareServices.DataAccess.Repository;
using AppShareServices.Mappings;
using AppShareServices.Pagging;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WorkerApplication.ViewModels;
using WorkerDomain.AgreegateModels.WorkerAgreegate;
using Worker = WorkerDomain.AgreegateModels.WorkerAgreegate.Worker;

namespace WorkerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeKeepingsController : ControllerBase
    {
        private readonly ILogger<WorkersController> _logger;
        private readonly IRepositoryService _repositoryService;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMappingService _mappingService;
        private readonly IUriService _uriService;

        public TimeKeepingsController(ILogger<WorkersController> logger,
            IMappingService mappingService,
            IUriService uriService,
            IRepositoryService repositoryService,
            ICommandDispatcher commandDispatcher
            )
        {
            _logger = logger;
            _uriService = uriService;
            _mappingService = mappingService;
            _repositoryService = repositoryService;
            _commandDispatcher = commandDispatcher;
        }

        /// <summary>
        /// Add worker checkin a shift in working day or update if existing
        /// </summary>
        /// <param name="workerStartShiftVM"></param>
        /// <returns></returns>
        [HttpPost("checkin")]
        public async Task<IActionResult> WorkerStartShift([FromBody] WorkerStartShiftVM workerStartShiftVM)
        {
            var worker = _repositoryService.Find<Worker>(workerStartShiftVM.WorkerId);
            if (worker == null)
            {
                return BadRequest(@"Worker not found");
            }

            var shift = _repositoryService.Find<Shift>(workerStartShiftVM.ShiftId);

            if (shift == null)
            {
                return BadRequest(@"Shift not found");
            }

            // TODO: Case Shift have time of next date?
            Expression<Func<WorkerShift, bool>> @where = w => w.WorkerId == worker.Id && w.ShiftId == shift.Id && w.DateStarted.Date == workerStartShiftVM.DateStarted.Date;
            var workerShiftExisting = _repositoryService.Find(@where);

            if (workerShiftExisting == null)
            {
                var workerShift = WorkerShift.CreateStartShift(worker.Id, shift.Id, workerStartShiftVM.DateStarted);
                _repositoryService.Add<WorkerShift>(workerShift);
            }
            else
            {
                _repositoryService.Update(workerShiftExisting.UpdateStartShift(workerStartShiftVM.DateStarted));
            }

            _repositoryService.SaveChanges();

            return Ok();
        }

        [HttpPut("checkout")]
        public async Task<IActionResult> WorkerEndShift([FromBody] WorkerEndShiftVM workerEndShiftVM)
        {
            var worker = _repositoryService.Find<Worker>(workerEndShiftVM.WorkerId);
            if (worker == null)
            {
                return BadRequest(@"Worker not found");
            }

            var shift = _repositoryService.Find<Shift>(workerEndShiftVM.ShiftId);

            if (shift == null)
            {
                return BadRequest(@"Shift not found");
            }

            // TODO: Case Shift have time of next date?
            Expression<Func<WorkerShift, bool>> @where = w => w.WorkerId == worker.Id && w.ShiftId == shift.Id && w.DateStarted.Date == workerEndShiftVM.DateEnded.Date;
            var workerShiftExisting = _repositoryService.Find(@where);

            if (workerShiftExisting == null)
            {
                return BadRequest("Worker have not started working in this shift");
            }

            _repositoryService.Update<WorkerShift>(workerShiftExisting.UpdateEndShift(shift, workerEndShiftVM.DateEnded));
            _repositoryService.SaveChanges();

            return Ok();
        }
    }
}
