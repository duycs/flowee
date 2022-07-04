using System;
namespace ProductApplication.ViewModels
{
	public class UpdateProductVM
	{
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? PriceStandar { get; set; }
        public int? QuantityAvailable { get; set; }
        public int? SpecificationId { get; set; }
        public int[]? AddonIds { get; set; }
        public int[]? CategoryIds { get; set; }
    }
}

