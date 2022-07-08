using AppShareDomain.Models;
using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class JobStepStatus : Enumeration
    {
        public static JobStepStatus None = new JobStepStatus(0, nameof(None).ToLowerInvariant());
        public static JobStepStatus WaitingAssign = new JobStepStatus(1, nameof(WaitingAssign).ToLowerInvariant());
        public static JobStepStatus Assigned = new JobStepStatus(2, nameof(Assigned).ToLowerInvariant());
        public static JobStepStatus Done = new JobStepStatus(3, nameof(Done).ToLowerInvariant());

        public JobStepStatus(int id, string name) : base(id, name)
        {
        }
    }
}
