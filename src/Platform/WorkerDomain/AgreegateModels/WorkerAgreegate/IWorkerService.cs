using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public interface IWorkerService
    {
        Worker Add(Worker worker);
        Worker Remove(Worker worker);
    }
}
