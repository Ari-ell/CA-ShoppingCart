using System;
namespace CA_ShoppingWebsite.Models
{
	public class Transaction
	{
        public string? ID { get; set; }
        public string? UserID { get; set; }
        public string? PurchaseDate { get; set; }

        // Activation code should also be a list as there can be multiple codes
        public string? ActivationCode { get; set; }
        public List<Guid>? ActivationCode1 { get; set; } // alt

        // Maybe can replace this with a list instead?
        public string? ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDesc { get; set; }
        public List<Products>? PurchasedProducts { get; set; } // alt
    }
}

