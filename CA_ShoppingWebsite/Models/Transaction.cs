using System;
namespace CA_ShoppingWebsite.Models
{
	public class Transaction
	{
        public string? ID { get; set; }
        public string? UserID { get; set; }
        public string? ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDesc { get; set; }
        public string? PurchaseDate { get; set; }
        public string? ActivationCode { get; set; }

    }
}

