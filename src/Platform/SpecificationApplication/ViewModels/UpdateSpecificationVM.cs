using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecificationApplication.ViewModels
{
    public class UpdateSpecificationVM
    {
        [Required(ErrorMessage = "The id is required")]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int[]? RuleIds { get; set; }
    }
}
