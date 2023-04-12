using System;
namespace CA_ShoppingWebsite.Models;

public class Review
{
	public Review() { }

	public int? UserId { get; set; }
	public int? ProductId { get; set; }
	public int? Rating { get; set; }
}

