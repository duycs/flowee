using System;
using System.ComponentModel.DataAnnotations;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.OrderAgreegate
{
    public class Customer : Entity
    {
        [Required]
        [MaxLength(36)]
        public string Code { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
        public PriorityLevel? PriorityLevel { get; set; }
    }
}

