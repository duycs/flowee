using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AppShareDomain.Models;
using AppShareServices.Models;

namespace CatalogDomain.AgreegateModels.CatalogAgreegate
{
    /// <summary>
    /// Catalog is product standar/productLevel
    /// Can add more Addons for Product Standar
    /// </summary>
    public class Catalog : Entity, IAggregateRoot
    {
        [Required]
        [MaxLength(36)]
        public string Code { get; set; }

        [MaxLength(250)]
        public string? Name { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public decimal PriceStandar { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// Quantity available in stock
        /// </summary>
        public int QuantityAvailable { get; set; }

        /// <summary>
        /// Product standar can have n Addons
        /// </summary>
        public virtual ICollection<Addon>? Addons { get; set; }

        /// <summary>
        /// Link to Specification where defiend this product
        /// </summary>
        public int SpecificationId { get; set; }

        [JsonIgnore]
        public virtual ICollection<CatalogAddon>? CatalogAddons { get; set; }

        public static Catalog Create(string name, string? description, decimal priceStandar, int quantityAvailable, List<Addon>? addons)
        {
            return new Catalog()
            {
                Name = name,
                Description = description,
                PriceStandar = priceStandar,
                QuantityAvailable = quantityAvailable,
                Addons = addons
            };
        }

        public decimal CanculatePrice()
        {
            if (Addons != null && Addons.Any())
            {
                decimal totalAddonPrice = Addons.Sum(i => i.Price);
                return PriceStandar + totalAddonPrice;
            }

            return PriceStandar;
        }

        public bool IsAvaliableInStock()
        {
            return QuantityAvailable > 0;
        }
    }
}

