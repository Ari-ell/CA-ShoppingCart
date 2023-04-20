using System;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Data;

public class GalleryData
{
    // Get list of products to be displayed based on search keyword
    public static List<Product> Search(string keyword, List<Product> products)
    {
        if (keyword == "" || keyword == null)
        {
            return products;
        }
        List<Product> selected = new List<Product>();

        foreach (Product product in products)
        {
            if (product.Name != null)
            {
                if (product.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                    product.Description.Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                {
                    selected.Add(product);
                }
            }
        }
        return selected;
    }

    // Returns quantity of items in cart
    // from either cookies or CartItem table
    // based on whteher the user is logged in or not
    public static int checkQty(HttpRequest request, string userId)
    {
        int cartCounter = 0;

        // Query DB for cart item qty based on userId
        if (userId != null)
        {
            MySqlConnection conn = new MySqlConnection(data.cloudDB);
            conn.Open();
            string countQuery = $"SELECT SUM(Quantity) FROM cartItem WHERE UserId = \"{userId}\"";
            MySqlCommand countQty = new MySqlCommand(countQuery, conn);
            MySqlDataReader resQty = countQty.ExecuteReader();
            
            resQty.Read();

            cartCounter = resQty[0].ToString() == "" ? 0 : Convert.ToInt32(resQty[0]);
            
            conn.Close();
        }
        // If user is not logged in,
        // Get qty of items based on cookies
        else
        {
            if (request.Cookies.Count() > 0)
            {
                foreach (KeyValuePair<string, string> c in request.Cookies)
                {
                    if (c.Key != "SessionId" && c.Key != "userID" && c.Key != "name" && c.Key != "username")
                    {
                        cartCounter += Convert.ToInt32(c.Value);
                    }
                }
            }
        }
        return cartCounter;
    }
}

