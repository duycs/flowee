using AppShareServices.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogDomain.AgreegateModels.CatalogAgreegate
{
	public class Addon : Entity
	{
        [MaxLength(36)]
        public string Code { get; set; }
        [MaxLength(250)]
        public string? Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Specification defiend how to made this Addon
        /// </summary>
        public int? SpecificationId { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<Catalog>? Catalogs { get; set; }

        [JsonIgnore]
        public virtual ICollection<CatalogAddon>? CatalogAddons { get; set; }
    }
}

