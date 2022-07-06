using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApplication.DTOs
{
    public class CatalogDto : DtoBase
    {
        public string Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// Quantity available in stock
        /// </summary>
        public int? QuantityAvailable { get; set; }

        /// <summary>
        /// Price of Product in standar
        /// </summary>
        public decimal? PriceStandar { get; set; }

        /// <summary>
        /// Total price of Product standar and price of Addons
        /// </summary>
        public decimal? Price { get; set; }

        public int? CurrencyId { get; set; }
        public string? Currency { get; set; }

        /// <summary>
        /// Specification of Product standar and Addons
        /// </summary>
        public ICollection<SpecificationDto>? Specifications { get; set; }

        /// <summary>
        /// Product standar can have n Addons
        /// </summary>
        public virtual ICollection<AddonDto>? Addons { get; set; }
    }
}
