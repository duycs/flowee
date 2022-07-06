using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace CatalogDomain.AgreegateModels.CatalogAgreegate
{
    public class CatalogAddon : Entity
    {
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public int AddonId { get; set; }
        public Addon Addon { get; set; }
    }
}

