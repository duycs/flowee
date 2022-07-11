using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationApplication.ViewModels
{
    public class CreateSpecificationVM
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int[]? RuleIds { get; set; }
    }
}
