using System;
namespace CA_ShoppingWebsite.Models;

public class PurchaseOrder
{
    public string? PurchaseId {get; set;} 
    public string? UserId { get; set; }
    public string? ProductId { get; set; }
    public int? PurchaseQty { get; set; }
    public string? PurchaseDate { get; set; }
}

