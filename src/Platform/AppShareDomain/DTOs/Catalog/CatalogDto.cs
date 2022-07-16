using AppShareDomain.DTOs.Specification;

namespace AppShareDomain.DTOs.Catalog
{
    public class CatalogDto : DtoBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Quantity available in stock
        /// </summary>
        public int QuantityAvailable { get; set; }

        /// <summary>
        /// Price of Product in standar
        /// </summary>
        public decimal? PriceStandar { get; set; }

        /// <summary>
        /// Total price of Product standar and price of Addons
        /// </summary>
        public decimal Price { get; set; }

        public EnumerationDto Currency { get; set; }

        public int SpecificationId { get; set; }

        /// <summary>
        /// Specification of Product standar and Addons
        /// </summary>
        public SpecificationDto Specification { get; set; }

        /// <summary>
        /// Product standar can have n Addons
        /// </summary>
        public List<AddonDto> Addons { get; set; }
    }
}
