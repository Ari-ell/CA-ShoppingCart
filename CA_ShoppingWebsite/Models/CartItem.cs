namespace CA_ShoppingWebsite.Models
{
    public class CartItem
    {
        public int? CartItemId { get; set; }

        public string? UserId { get; set; }

        public string? ProductId { get; set; }

        public int? Quantity { get; set; }   
    }
}
