using AppShareDomain.Models;

namespace JobDomain.AgreegateModels.JobAgreegate
{
    /// <summary>
    /// Collection Transition data struct stype
    /// </summary>
    public class TransitionType : Enumeration
    {
        public static TransitionType Next = new TransitionType(1, nameof(Next).ToLowerInvariant());
        public static TransitionType Back = new TransitionType(2, nameof(Back).ToLowerInvariant());
        public static TransitionType Retry = new TransitionType(3, nameof(Retry).ToLowerInvariant());
        public TransitionType(int id, string name) : base(id, name)
        {
        }
    }
}