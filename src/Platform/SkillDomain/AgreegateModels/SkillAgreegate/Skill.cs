using AppShareDomain.Models;
using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkillDomain.AgreegateModels.SkillAgreegate
{
    public class Skill : Entity, IAggregateRoot
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<MatrixSkill> MatrixSkills { get; set; }

        [JsonIgnore]
        public virtual ICollection<SkillOperations> SkillOperations { get; set; } = new List<SkillOperations>();

        public List<Guid> GetOperationIds()
        {
            return SkillOperations.Select(o => o.OperationId).ToList();
        }
    }
}
