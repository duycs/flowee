using System;
namespace ProductApplication.ViewModels
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int[]? CategoryIds { get; set; }

        /// <summary>
        /// Catalog has all data of product
        /// </summary>
        public int? CatalogId { get; set; }
    }
}

