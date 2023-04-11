using System;
namespace CA_ShoppingWebsite.Models
{
	public class User
	{
	
        public string? ID { get; set; }
        public string? Username { get; set; }
        public List<Products>? Car { get; set; }
        public List<Products>? History { get; set; }
  
	}
}

