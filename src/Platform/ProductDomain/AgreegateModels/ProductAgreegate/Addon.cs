using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
	public class Addon : Entity
	{
        [MaxLength(36)]
        public string Code { get; set; }
        [MaxLength(2250)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductAddon>? ProductAddons { get; set; }
    }
}

