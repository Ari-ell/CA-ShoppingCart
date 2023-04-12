using System;
namespace CA_ShoppingWebsite.Models
{
	public class Purchases
	{
        public Purchases() { }

        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public string? PurchaseDate { get; set; }
        public Guid ActivationCode { get; set; }
    }
}

