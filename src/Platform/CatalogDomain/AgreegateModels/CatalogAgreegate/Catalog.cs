using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AppShareDomain.Models;
using AppShareServices.Models;
using CatalogDomain.AgreegateModels.SpecificationAgreegate;

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

        /// <summary>
        /// Quantity available in stock
        /// </summary>
        public int QuantityAvailable { get; set; }

        /// <summary>
        /// Price of Product in standar
        /// </summary>
        public decimal PriceStandar { get; set; }

        /// <summary>
        /// Total price of Product standar and price of Addons
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Specification defiend how to made this Product standar
        /// </summary>
        public int SpecificationId { get; set; }

        /// <summary>
        /// Specification of Product standar and Addons
        /// </summary>
        public ICollection<int>? SpecificationIds { get; set; }


        /// <summary>
        /// Product standar can have n Addons
        /// </summary>
        public virtual ICollection<Addon>? Addons { get; set; }

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

        public List<int> GetSpecifications()
        {
            var specifications = new List<int>();
            var specificationProductStandar = new List<int>() { SpecificationId };
            if (Addons != null && Addons.Any())
            {
                var specificationAddons = Addons.Select(i => i.SpecificationId).ToList();
                specificationProductStandar.AddRange(specificationAddons);
            }

            return specifications;
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

