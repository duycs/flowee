using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AppShareDomain.Models;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
    /// <summary>
    /// Product standar or Catalog
    /// </summary>
    public class Product : Entity, IAggregateRoot
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
        /// Product level is determined by Product Standar and ProductAddon
        /// </summary>
        public int? ProductLevelId { get; set; }
        public ProductLevel? ProductLevel { get; set; }

        public virtual ICollection<Category>? Categories { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductAddon>? ProductAddons { get; set; }

        /// <summary>
        /// Product standar can have n Addons
        /// </summary>
        public virtual ICollection<Addon>? Addons { get; set; }

        public static Product Create(string name, string? description, decimal priceStandar, int quantityAvailable, List<Category>? categories, List<Addon>? addons)
        {
            return new Product()
            {
                Name = name,
                Description = description,
                PriceStandar = priceStandar,
                QuantityAvailable = quantityAvailable,
                Categories = categories,
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

        public ProductLevel CanculateProductLevel()
        {
            // TODO: matrix productStandar-Addons => ProductLevel
            return ProductLevel.Level1;
        }
    }
}

