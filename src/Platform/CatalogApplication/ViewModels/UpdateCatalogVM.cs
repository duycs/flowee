namespace CatalogApplication.ViewModels
{
    public class UpdateCatalogVM
    {
        public int Id { get; set; }
        public string? Code { get; set; }
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

        public int? CurrencyId { get; set; }

        /// <summary>
        /// Specification defiend how to made this Product standar
        /// </summary>
        public int? SpecificationId { get; set; }

        /// <summary>
        /// List of addon for this product
        /// </summary>
        public int[]? AddonIds { get; set; }
    }
}
