using AppShareDomain.DTOs.Skill;
using System.Text.Json.Serialization;

namespace AppShareDomain.DTOs.Specification
{
    public class SpecificationSkillDto : DtoBase
    {
        //public int SpecificationId { get; set; }
        //public SpecificationDto Specification { get; set; }
        public int? SkillId { get; set; }
        public SkillDto? Skill { get; set; }
        public int? SkillLevelId { get; set; }
        public EnumerationDto? SkillLevel { get; set; }
    }
}
