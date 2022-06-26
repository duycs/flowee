using System;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
    public class ProductCategories : Entity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

