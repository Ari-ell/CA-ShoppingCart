using System;
namespace CA_ShoppingWebsite.Models;

public class PurchaseList
{
	public int? ProductId { get; set; }
	public string? PurchaseId { get; set; }
	public Guid? ActivationCode { get; set; }
}

