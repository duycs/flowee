using AppShareDomain.Models;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    /// <summary>
    /// level up
    /// </summary>
    public class SkillLevel : Enumeration
    {
        public static SkillLevel Level0 = new SkillLevel(1, nameof(Level0).ToLowerInvariant());
        public static SkillLevel Level1 = new SkillLevel(2, nameof(Level1).ToLowerInvariant());
        public static SkillLevel Level2 = new SkillLevel(3, nameof(Level2).ToLowerInvariant());
        public static SkillLevel Level3 = new SkillLevel(4, nameof(Level3).ToLowerInvariant());

        public SkillLevel(int id, string name) : base(id, name)
        {
        }
    }
}
