using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class StepStatus : Enumeration
    {
        public static StepStatus None = new StepStatus(1, nameof(None).ToLowerInvariant());
        public static StepStatus WaitingAssign = new StepStatus(2, nameof(WaitingAssign).ToLowerInvariant());
        public static StepStatus Assigned = new StepStatus(3, nameof(Assigned).ToLowerInvariant());
        public static StepStatus Done = new StepStatus(4, nameof(Done).ToLowerInvariant());

        public StepStatus(int id, string name) : base(id, name)
        {
        }
    }
}
