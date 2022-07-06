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
        [MaxLength(2250)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Catalog>? Catalogs { get; set; }

        [JsonIgnore]
        public virtual ICollection<CatalogAddon>? CatalogAddons { get; set; }
    }
}

