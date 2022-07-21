using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkerDomain.AgreegateModels.WorkerAgreegate
{
    public class WorkerSkill : Entity
    {
        [Required]
        [Column(Order = 0)]
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        [Required]
        [Column(Order = 1)]
        public int SkillId { get; set; }

        public int? SkillLevelId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPriority { get; set; }
    }
}
