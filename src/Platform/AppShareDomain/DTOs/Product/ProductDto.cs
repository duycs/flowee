using AppShareDomain.DTOs.Catalog;

namespace AppShareDomain.DTOs.Product
{
    public class ProductDto : DtoBase
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<CategoryDto>? Categories { get; set; }
        public int? CatalogId { get; set; }
        public CatalogDto? Catalog { get; set; }
        public string? Instruction { get; set; }
    }
}
