using System;
using System.ComponentModel.DataAnnotations;
using AppShareDomain.Models;
using AppShareServices.Models;
using ProductDomain.AgreegateModels.ProductAgreegate;

namespace ProductDomain.AgreegateModels.OrderAgreegate
{
	public class Order : Entity, IAggregateRoot
	{
        [Required]
        [MaxLength(36)]
        public string Code { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int CustomerId{ get; set; }
        public Customer Customer { get; set; }

        public ICollection<Product> Products { get; set; }

        public float? TotalPrice { get; set; }
    }
}

