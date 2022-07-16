using AppShareDomain.DTOs;
using AppShareServices.Models;

namespace CatalogApplication.DTOs
{
    public class AddonDto : DtoBase
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public int? SpecificationId { get; set; }

        /// <summary>
        /// Specification defiend how to made this Addon
        /// </summary>
        public SpecificationDto Specification { get; set; }

        public decimal? Price { get; set; }

        public EnumerationDto Currency { get; set; }
    }
}

