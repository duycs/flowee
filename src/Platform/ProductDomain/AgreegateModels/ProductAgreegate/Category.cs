using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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

        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }

        public Category Create(string code, string name, string description)
        {
            return new Category()
            {
                Code = code,
                Name = name,
                Description = description
            };
        }

        public Category PathUpdate(string? code, string? name, string? description)
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

            return this;
        }
    }
}

