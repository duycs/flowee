using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerApplication.ViewModels
{
    public class CreateCustomerVM
    {
        [MaxLength(250)]
        public string? FirstName { get; set; }
        [MaxLength(250)]
        public string? LastName { get; set; }
        [MaxLength(500)]
        public string? FullName { get; set; }
        [MaxLength(36)]
        public string? Code { get; set; }
        [MaxLength(250)]
        public string? Email { get; set; }
        [MaxLength(36)]
        public string? Phone { get; set; }
        public string? Description { get; set; }

        public int? CurrencyId { get; set; }

        public int? PriorityLevelId { get; set; }

        public int[]? PaymentMethodIds { get; set; }
    }
}
