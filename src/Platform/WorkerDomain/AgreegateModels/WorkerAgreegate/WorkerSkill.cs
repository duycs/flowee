using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class WorkerSkill : Entity
    {
        [Required]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        [Required]
        public int? SkillId { get; set; }

        public int? SkillLevelId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPriority { get; set; }

        public static WorkerSkill Create(int workerId, int skillId, int skillLevelId, bool isActive = false, bool isPriority = false)
        {
            return new WorkerSkill()
            {
                WorkerId = workerId,
                SkillId = skillId,
                SkillLevelId = skillLevelId,
                IsActive = isActive,
                IsPriority = isPriority
            };
        }
    }
}
