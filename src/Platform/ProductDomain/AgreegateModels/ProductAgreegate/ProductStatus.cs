using System;
using AppShareServices.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
    public class ProductStatus : Enumeration
    {
        public static ProductStatus Backlog = new ProductStatus(0, nameof(Backlog).ToLowerInvariant());
        public static ProductStatus Todo = new ProductStatus(1, nameof(Todo).ToLowerInvariant());
        public static ProductStatus Doing = new ProductStatus(2, nameof(Doing).ToLowerInvariant());
        public static ProductStatus QCCheck = new ProductStatus(3, nameof(QCCheck).ToLowerInvariant());
        public static ProductStatus Finish = new ProductStatus(4, nameof(Finish).ToLowerInvariant());

        public ProductStatus(int id, string name) : base(id, name)
        {
        }
    }
}