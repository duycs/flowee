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
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<Category>? Categories { get; set; }

        public ICollection<ProductCategory>? ProductCategories { get; set; }

        /// <summary>
        /// Catalog has all data of product
        /// </summary>
        public int? CatalogId { get; set; }

        /// <summary>
        /// Instruction description overall how to made this product
        /// Deductive from specifications of catalog
        /// </summary>
        public string? Instruction { get; set; }

        public static Product Create(string code, string name, string description, int? catalogId, List<Category> categories)
        {
            return new Product()
            {
                Code = code,
                Name = name,
                Description = description,
                CatalogId = catalogId,
                Categories = categories
            };
        }

        public Product PathUpdate(string? code, string? name, string? description, int? catalogId, List<Category>? categories)
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

            if (catalogId is not null)
            {
                CatalogId = catalogId;
            }

            if (categories is not null)
            {
                Categories = categories;
            }

            return this;
        }

        public Product SetInstruction(string instruction)
        {
            Instruction = instruction;
            return this;
        }

    }
}

