using System;
namespace CA_ShoppingWebsite.Models
{
	public class User
	{
	
        public string? ID { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public List<Products>? Cart { get; set; }
        public List<Transaction>? History { get; set; }
  
	}
}

