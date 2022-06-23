using AppShareServices.Commands;
using AutoMapper;
using JobDomain.AgreegateModels.WorkerAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerApplication.ViewModels;
using WorkerDomain.Commands;

namespace WorkerApplication.Services
{
    public class WorkerManager : IWorkerManager
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IMapper _mapper;

        public WorkerManager(ICommandDispatcher commandDispatcher, IMapper mapper)
        {
            _commandDispatcher = commandDispatcher;
            _mapper = mapper;
        }

        public async Task AddWorkerAsync(CreateWorkerVM createWorkerVM)
        {
            var createWorkerCommand = _mapper.Map<CreateWorkerCommand>(createWorkerVM);
            await _commandDispatcher.Send(createWorkerCommand);
        }
    }
}
