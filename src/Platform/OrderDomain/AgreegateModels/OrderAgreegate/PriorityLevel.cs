using AppShareDomain.Models;

namespace OrderDomain.AgreegateModels.OrderAgreegate
{
    public class PriorityLevel : Enumeration
    {
        public static PriorityLevel Level1 = new PriorityLevel(0, nameof(Level1).ToLowerInvariant());
        public static PriorityLevel Level2 = new PriorityLevel(1, nameof(Level2).ToLowerInvariant());
        public static PriorityLevel Level3 = new PriorityLevel(2, nameof(Level3).ToLowerInvariant());
        public static PriorityLevel Level4 = new PriorityLevel(3, nameof(Level4).ToLowerInvariant());

        public PriorityLevel(int id, string name) : base(id, name)
        {
        }
    }
}