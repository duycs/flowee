using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
	public class ProductAddon : Entity
	{
        public int ProductId { get; set; }
		public Product Product { get; set; }
		public int AddonId { get; set; }
		public Addon Addon { get; set; }
	}
}

