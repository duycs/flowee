using AppShareDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDomain.AgreegateModels.CustomerAgreegate
{
    public class PriorityLevel : Enumeration
    {
        public static PriorityLevel Level0 = new PriorityLevel(0, nameof(Level0).ToLowerInvariant());
        public static PriorityLevel Level1 = new PriorityLevel(1, nameof(Level1).ToLowerInvariant());
        public static PriorityLevel Level2 = new PriorityLevel(2, nameof(Level2).ToLowerInvariant());
        public static PriorityLevel Level3 = new PriorityLevel(2, nameof(Level3).ToLowerInvariant());

        public PriorityLevel(int id, string name) : base(id, name)
        {
        }
    }
}
