using System;
using AppShareDomain.Models;

namespace ProductDomain.AgreegateModels.ProductAgreegate
{
	public class ProductLevel : Enumeration
	{
        public static ProductLevel Level1 = new(1, nameof(Level1));
        public static ProductLevel Level2 = new(2, nameof(Level2));
        public static ProductLevel Level3 = new(3, nameof(Level3));

        public ProductLevel(int id, string name) : base(id, name)
        {
        }
    }
}

