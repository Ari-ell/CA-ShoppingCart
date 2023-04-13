using System;
using CA_ShoppingWebsite.Models;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Data;

public class ProductData
{
	public static List<Models.Product>? GetAllProducts()
	{
		return null;
	}

	public static Models.Product? GetProductToAddToCart()
	{
		return null;
	}


    // Get product details to be displayed on MyPurchases view
	public static List<Models.Product>? GetProductDetails(int userId)
	{
        var products = new List<Models.Product>();
        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            string sql = @"SELECT Name, Description, Img
                        FROM Product, PurchaseOrder
                        WHERE Product.ProductId IN
                        (SELECT ProductId 
                        FROM PurchaseOrder
                        WHERE UserId = " + userId;
            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Models.Product
                {
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Img = (string)reader["Img"],
                    Price = (double)reader["Price"],
                };
                products.Add(product);
            }
            conn.Close();
        }
        return products;
    }
}

