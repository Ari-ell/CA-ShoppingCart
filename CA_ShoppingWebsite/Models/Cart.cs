using System;
namespace CA_ShoppingWebsite.Models;

public class Cart
{
	public int? UserId { get; set; }
	public int? ProductId { get; set; }
	public int? ProductQty { get; set; }
}

