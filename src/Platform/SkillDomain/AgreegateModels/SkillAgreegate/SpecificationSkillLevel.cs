using AppShareDomain.Models;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class SpecificationSkillLevel : Enumeration
    {
        public static SpecificationSkillLevel Level1 = new SpecificationSkillLevel(1, nameof(Level1).ToLowerInvariant());
        public static SpecificationSkillLevel Level2 = new SpecificationSkillLevel(2, nameof(Level2).ToLowerInvariant());
        public static SpecificationSkillLevel Level3 = new SpecificationSkillLevel(3, nameof(Level3).ToLowerInvariant());
        public static SpecificationSkillLevel Level4 = new SpecificationSkillLevel(4, nameof(Level4).ToLowerInvariant());

        public SpecificationSkillLevel(int id, string name) : base(id, name)
        {
        }
    }
}
