using AppShareDomain.DTOs.Catalog;

namespace AppShareDomain.DTOs.Product
{
    public class ProductDto : DtoBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CategoryDto>? Categories { get; set; }

        public int? CatalogId { get; set; }

        /// <summary>
        /// Catalog has all data of product
        /// </summary>
        public CatalogDto? Catalog { get; set; }

        /// <summary>
        /// Instruction description overall how to made this product
        /// Deductive from specifications of catalog
        /// </summary>
        public string? Instruction { get; set; }
    }
}
