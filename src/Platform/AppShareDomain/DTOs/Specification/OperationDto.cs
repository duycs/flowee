using AppShareDomain.DTOs.Catalog;
using AppShareDomain.DTOs.Skill;
using System;
namespace AppShareDomain.DTOs.Specification
{
    public class OperationDto : DtoBase
    {
        /// <summary>
        /// Guid for public global function
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Function name for invoke
        /// </summary>
        public string Function { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public EnumerationDto State { get; set; }
        public List<RuleDto>? Rules { get; set; }
        public List<SkillDto> Skills { get; set; }
    }
}

