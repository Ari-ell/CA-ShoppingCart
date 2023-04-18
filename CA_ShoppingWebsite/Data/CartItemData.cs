using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using Google.Protobuf.Compiler;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using CA_ShoppingWebsite.Data;

namespace CA_ShoppingWebsite.Data;

public class CartItemData {

    public static Dictionary<Product, int> GetProductList(string userId) {
        Dictionary<Product, int> ProductList = new Dictionary<Product, int>();

        using (var connection = new MySqlConnection(data.cloudDB)) {
            connection.Open();

            string sql = @"SELECT p.ProductId, p.Img, p.Name, p.Description, p.Price, c.Quantity FROM cartitem c, product p
                           WHERE c.ProductId = p.ProductId
                           AND UserId = @userId";


            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@userId", userId);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                Product product = new Product();

                product.ProductId = (string)reader["ProductId"];
                product.Img = (string)reader["Img"];
                product.Name = (string)reader["Name"];
                product.Description = (string)reader["Description"];
                product.Price = (int)reader["Price"];
                int qty = (int)reader["Quantity"];

                ProductList.Add(product, qty);
            }
            connection.Close();
        }
        return ProductList;
    }

}

