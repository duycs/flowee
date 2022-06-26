using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;
using ProductDomain.AgreegateModels.OrderAgreegate;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
    public class Product : Entity, IAggregateRoot
    {
        [Required]
        [MaxLength(36)]
        public string Code { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }
        public ProductStatus? ProductStatus { get; set; }

        [MaxLength(2000)]
        public string ExpectResult { get; set; }

        public PriorityLevel? PriorityLevel { get; set; }
        public int? ComplexScore { get; set; }

        public virtual ICollection<Category>? Categories { get; set; }
    }
}

