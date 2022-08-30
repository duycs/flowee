using AppShareDomain.DTOs;
using AppShareDomain.DTOs.Job;
using AppShareDomain.DTOs.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareDomain.DTOs.Catalog
{
    public class RuleDto : DtoBase
    {
        public bool IsNot { get; set; }

        public int? ConditionId { get; set; }
        public EnumerationDto? Condition { get; set; }

        public int SettingId { get; set; }
        public SettingDto Setting { get; set; }

        public int? OperatorId { get; set; }
        public EnumerationDto? Operator { get; set; }

        public string Value { get; set; }

        public List<OperationDto>? Operations { get; set; }
    }
}
