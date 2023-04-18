using System;
namespace CA_ShoppingWebsite.Models;

public class Review
{
	public string? UserId { get; set; }
	public string? ProductId { get; set; }
	public int? Rating { get; set; }
}

