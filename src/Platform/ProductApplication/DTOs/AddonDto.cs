using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductApplication.DTOs
{
    public class AddonDto : DtoBase
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// Specification defiend how to made this Addon
        /// </summary>
        public int? SpecificationId { get; set; }

        public decimal? Price { get; set; }

        public int? CurrencyId { get; set; }
        public string Currency { get; set; }
    }
}

