using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class JobStatus : Enumeration
    {
        public static JobStatus None = new JobStatus(1, nameof(None).ToLowerInvariant());
        public static JobStatus Backlog = new JobStatus(2, nameof(Backlog).ToLowerInvariant());
        public static JobStatus Todo = new JobStatus(3, nameof(Todo).ToLowerInvariant());
        public static JobStatus Doing = new JobStatus(4, nameof(Doing).ToLowerInvariant());
        public static JobStatus Pending = new JobStatus(5, nameof(Pending).ToLowerInvariant());
        public static JobStatus QCCheck = new JobStatus(6, nameof(QCCheck).ToLowerInvariant());
        public static JobStatus Finish = new JobStatus(7, nameof(Finish).ToLowerInvariant());

        public JobStatus(int id, string name) : base(id, name)
        {
        }
    }
}
