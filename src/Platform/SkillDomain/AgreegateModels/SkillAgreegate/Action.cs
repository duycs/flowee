using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class Action : Entity
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}
