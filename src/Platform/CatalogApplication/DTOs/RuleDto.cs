using AppShareDomain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogApplication.DTOs
{
    public class RuleDto
    {
        public bool IsNot { get; set; }

        public EnumerationDto Condition { get; set; }

        public SettingDto Setting { get; set; }

        public string Value { get; set; }
    }
}
