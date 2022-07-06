using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApplication.DTOs
{
    public class ProductDto : DtoBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CategoryDto>? Categories { get; set; }

        /// <summary>
        /// Catalog has all data of product
        /// </summary>
        public CatalogDto Catalog { get; set; }

        /// <summary>
        /// Instruction description overall how to made this product
        /// Deductive from specifications of catalog
        /// </summary>
        public string? Instruction { get; set; }
    }
}
