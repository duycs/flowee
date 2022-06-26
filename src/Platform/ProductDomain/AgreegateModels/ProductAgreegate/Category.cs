using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
    public class Category : Entity
    {
        [Required]
        [MaxLength(36)]
        public string Code { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<Category>? Categories { get; set; }
    }
}

