using AppShareDomain.DTOs.Operation;
using AppShareDomain.DTOs.Specification;

namespace AppShareDomain.DTOs.Skill
{
    public class SkillDto : DtoBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OperationDto>? Operations { get; set; }
    }
}
