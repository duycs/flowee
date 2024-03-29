﻿using System;
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

        /// <summary>
        /// Quantity available in stock
        /// </summary>
        public int? QuantityAvailable { get; set; }

        /// <summary>
        /// Price of Product in standar
        /// </summary>
        public decimal? PriceStandar { get; set; }

        /// <summary>
        /// Total price of Product standar and price of Addons
        /// </summary>
        public decimal? Price { get; set; }

        public int? CurrencyId { get; set; }
        public Currency? Currency { get; set; }

        /// <summary>
        /// Specification defiend how to made this Product standar
        /// </summary>
        public int? SpecificationId { get; set; }

        /// <summary>
        /// Specification of Product standar and Addons
        /// </summary>
        private ICollection<int>? SpecificationIds { get; set; }

        /// <summary>
        /// Product standar can have n Addons
        /// </summary>
        public virtual ICollection<Addon>? Addons { get; set; }

        [JsonIgnore]
        public virtual ICollection<CatalogAddon>? CatalogAddons { get; set; }

        public static Catalog Create(string code, string name, decimal priceStandar, Currency currency, int? secificationId, int? quantityAvailable, string? description, List<Addon>? addons)
        {
            return new Catalog()
            {
                Code = code,
                Name = name,
                Description = description,
                PriceStandar = priceStandar,
                Currency = currency,
                SpecificationId = secificationId,
                QuantityAvailable = quantityAvailable,
                Addons = addons
            };
        }

        public Catalog PathUpdate(string? code, string? name, decimal? priceStandar, Currency? currency, int? secificationId, int? quantityAvailable, string? description, List<Addon>? addons)
        {
            if (code is not null)
            {
                Code = code;
            }

            if (name is not null)
            {
                Name = name;
            }

            if (description is not null)
            {
                Description = description;
            }

            if (priceStandar is not null)
            {
                PriceStandar = priceStandar;
            }

            if (currency is not null)
            {
                Currency = currency;
            }

            if (SpecificationId is not null)
            {
                SpecificationId = secificationId;
            }

            if (quantityAvailable is not null)
            {
                QuantityAvailable = quantityAvailable;
            }

            if (addons is not null)
            {
                Addons = addons;
            }

            return this;
        }

        /// <summary>
        /// Description from instruction of Specifications
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Catalog SetDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// List Specifications of product standar and addons
        /// </summary>
        /// <returns></returns>
        public int[] GetSpecificationIds()
        {
            var specifications = new List<int>();
            if (SpecificationId is not null && SpecificationId > 0)
            {
                specifications.Add(SpecificationId ?? 0);
            }

            if (Addons is not null && Addons.Any())
            {
                var specificationAddons = Addons.Where(i => i.SpecificationId.HasValue && i.SpecificationId > 0).Select(i => i.SpecificationId).Cast<int>().ToList();
                specifications.AddRange(specificationAddons);
            }

            return specifications.ToArray();
        }

        /// <summary>
        /// Sum total price of product standar and addons
        /// </summary>
        /// <returns></returns>
        public decimal CanculatePrice()
        {
            if (Addons is not null && Addons.Any())
            {
                decimal totalAddonPrice = Addons.Sum(i => i.Price ?? 0);
                return PriceStandar ?? 0 + totalAddonPrice;
            }

            return PriceStandar ?? 0;
        }

        public string PriceToString()
        {
            return string.Format("{0} {1}", CanculatePrice(), Currency?.ToString() ?? "");
        }

        public bool IsAvaliableInStock()
        {
            return QuantityAvailable > 0;
        }


    }
}

