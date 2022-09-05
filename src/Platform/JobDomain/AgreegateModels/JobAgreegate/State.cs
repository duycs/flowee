using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    public class State : Enumeration
    {
        public static State None = new State(1, nameof(None).ToLowerInvariant());
        public static State WaitingAssign = new State(2, nameof(WaitingAssign).ToLowerInvariant());
        public static State Assigned = new State(3, nameof(Assigned).ToLowerInvariant());
        public static State Done = new State(4, nameof(Done).ToLowerInvariant());

        public State(int id, string name) : base(id, name)
        {
        }
    }
}
