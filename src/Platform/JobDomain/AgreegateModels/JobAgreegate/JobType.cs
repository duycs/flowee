using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class JobType : Enumeration
    {
        public static JobType SingleWorker = new JobType(0, nameof(SingleWorker).ToLowerInvariant());
        public static JobType MultipleWorker = new JobType(1, nameof(MultipleWorker).ToLowerInvariant());
        public JobType(int id, string name) : base(id, name)
        {
        }
    }
}
