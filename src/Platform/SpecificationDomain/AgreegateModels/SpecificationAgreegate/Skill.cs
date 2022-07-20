using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SpecificationDomain.AgreegateModels.SpecificationAgreegate
{
    public class Skill : Entity
    {
        [MaxLength(36)]
        [Required]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<Specification>? Specifications { get; set; }

        [JsonIgnore]
        public virtual ICollection<SpecificationSkill>? SpecificationSkills { get; set; }
    }
}
