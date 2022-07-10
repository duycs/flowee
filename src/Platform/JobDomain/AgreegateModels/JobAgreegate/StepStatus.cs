using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class StepStatus : Enumeration
    {
        public static StepStatus None = new StepStatus(0, nameof(None).ToLowerInvariant());
        public static StepStatus WaitingAssign = new StepStatus(1, nameof(WaitingAssign).ToLowerInvariant());
        public static StepStatus Assigned = new StepStatus(2, nameof(Assigned).ToLowerInvariant());
        public static StepStatus Done = new StepStatus(3, nameof(Done).ToLowerInvariant());

        public StepStatus(int id, string name) : base(id, name)
        {
        }
    }
}
