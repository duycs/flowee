﻿using AppShareDomain.DTOs.Catalog;

namespace AppShareDomain.DTOs.Specification
{
    public class SpecificationDto : DtoBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Instruction { get; set; }
        public List<RuleDto> Rules { get; set; }
    }
}