using System;
namespace CA_ShoppingWebsite.Models
{
	public class Product
	{
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImg { get; set; }
        public double? ProductPrice { get; set; }
        public double? ProductReviewRating { get; set; }
    }
}

