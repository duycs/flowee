using System;
using System.ComponentModel.DataAnnotations;
using AppShareDomain.Models;
using AppShareServices.Models;

namespace OrderDomain.AgreegateModels.OrderAgreegate
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

        public ICollection<OrderItem>? OrderItems { get; set; }

        public float? TotalPrice { get; set; }
    }
}

