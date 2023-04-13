using System;
namespace CA_ShoppingWebsite.Models
{
    public class PurchaseOrder
    {
        public Guid? PurchaseId {get; set;}
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? PurchaseQty { get; set; }
        public string? PurchaseDate { get; set; }
    }
}

