using AppShareDomain.Models;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class WorkerSkillLevel : Enumeration
    {
        public static WorkerSkillLevel Level1 = new WorkerSkillLevel(1, nameof(Level1).ToLowerInvariant());
        public static WorkerSkillLevel Level2 = new WorkerSkillLevel(2, nameof(Level2).ToLowerInvariant());
        public static WorkerSkillLevel Level3 = new WorkerSkillLevel(3, nameof(Level3).ToLowerInvariant());
        public static WorkerSkillLevel Level4 = new WorkerSkillLevel(4, nameof(Level4).ToLowerInvariant());

        public WorkerSkillLevel(int id, string name) : base(id, name)
        {
        }
    }
}
