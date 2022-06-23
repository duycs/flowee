using JobDomain.AgreegateModels.WorkerAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerApplication.ViewModels;

namespace WorkerApplication.Services
{
    public interface IWorkerManager
    {
        public Task AddWorkerAsync(CreateWorkerVM createWorkerVM);
    }
}
