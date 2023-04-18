using System;
namespace CA_ShoppingWebsite.Models;

public class Product
{
    public string? ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Img { get; set; }
    public double? Price { get; set; }
    public double? ReviewRating { get; set; }
}

