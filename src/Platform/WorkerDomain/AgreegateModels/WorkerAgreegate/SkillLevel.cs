using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    /// <summary>
    /// level up
    /// </summary>
    public class SkillLevel : Enumeration
    {
        public static SkillLevel Level0 = new SkillLevel(0, nameof(Level0).ToLowerInvariant());
        public static SkillLevel Level1 = new SkillLevel(1, nameof(Level1).ToLowerInvariant());
        public static SkillLevel Level2 = new SkillLevel(2, nameof(Level2).ToLowerInvariant());
        public static SkillLevel Level3 = new SkillLevel(3, nameof(Level3).ToLowerInvariant());

        public SkillLevel(int id, string name) : base(id, name)
        {
        }
    }
}
