using JobDomain.AgreegateModels.ProductAgreegate;
using JobDomain.AgreegateModels.WorkerAgreegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class Job
    {
        public Product Product { get; set; }
        public List<Worker> Workers { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
