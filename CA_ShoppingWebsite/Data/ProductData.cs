﻿using System;
using CA_ShoppingWebsite.Models;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Data;

public class ProductData
{
	public static List<Models.Product>? GetAllProducts()
	{
        var products = new List<Models.Product>();
        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            string sql = @"SELECT *
                        FROM Product";
            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Models.Product
                {
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Img = (string)reader["Img"],
                    Price = (int)reader["Price"],
                    ReviewRating = (int)reader["ReviewRating"],
                    ProductId = (string)reader["ProductId"]
                };
                products.Add(product);
            }
            conn.Close();
        }
        return products;
    }
        
	public static Models.Product? GetProductToAddToCart()
	{
		return null;
	}

    // Get product details to be displayed on MyPurchases view
    // Returns a dictionary of relevant product info
    // with product as the key to matched against PurchaseOrder
    public static Dictionary<string, Product>? GetProductDetails(string userId)
	{
        var products = new Dictionary<string,Product>();
        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            string sql = @"SELECT DISTINCT Product.ProductId, Product.Name, Product.Description, Product.Img
                        FROM Product, PurchaseOrder
                        WHERE Product.ProductId IN
                            (SELECT PurchaseOrder.ProductId
                            FROM PurchaseOrder
                            WHERE PurchaseOrder.UserId = @userId);";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId",userId);

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var product = new Models.Product
                {
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                    Img = (string)reader["Img"],
                    //Price = (double)reader["Price"],
                };
                var productId = (string)reader["ProductId"];
                if (!products.ContainsKey(productId))
                    products[productId] = product;
            }
            conn.Close();
        }
        return products;
    }
}

