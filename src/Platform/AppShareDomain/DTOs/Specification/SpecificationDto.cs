using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Skill;

namespace AppShareDomain.DTOs.Specification
{
    public class SpecificationDto : DtoBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Instruction { get; set; }
        public List<RuleDto> Rules { get; set; }
        public int? SkillId { get; set; }
        public SkillDto? Skill { get; set; }

        public List<SpecificationSkillDto>? SpecificationSkills { get; set; }
    }
}
