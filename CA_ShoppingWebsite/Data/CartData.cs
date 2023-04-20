using System;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CA_ShoppingWebsite.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CA_ShoppingWebsite.Data;

public class CartData
{
    // Perform all actions needed to checkout user
    // 1. Convert cart items into list of PO objects
    // 2. Add PO list iteratively to PO records
    // 3. Add into PList records with actvCode
    // 4. Clear cart items


    // Edit qty of product in db 
    public static void EditCartQty(string? userID, string? productID, int? qty)
    {
        MySqlConnection conn = new MySqlConnection(data.cloudDB);
        try
        {
            conn.Open();

            // check if item exists in cart
            string querySql = "";
            if (qty > 0)
            {
                querySql = $"UPDATE cartitem SET quantity = {qty} WHERE productId = \"{productID}\" and UserId = \"{userID}\"";

            }
            else
            {
                querySql = $"DELETE from cartitem WHERE productId = \"{productID}\" and UserId = \"{userID}\"";

            }
            MySqlCommand queryCmd = new MySqlCommand(querySql, conn);
            queryCmd.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static void AddProductToCart(User user, string addProductId)
    {
        // establish connection to DB
        MySqlConnection conn = new MySqlConnection(data.cloudDB);

        try
        {
            conn.Open();

            // check if item exists in cart
            string querySql = $"SELECT * FROM cartItem WHERE cartItem.ProductId = \"{addProductId}\" AND cartItem.UserId = \"{user.UserId}\"";
            MySqlCommand queryCmd = new MySqlCommand(querySql, conn);
            MySqlDataReader rdr = queryCmd.ExecuteReader();
            string sqlQuery = "";

            // if item already exists in cart, update record quantity
            if (rdr.HasRows) 
            {
                sqlQuery = $"UPDATE cartitem SET quantity = quantity + 1 WHERE productId = \"{addProductId}\" and UserId = \"{user.UserId}\"";
            }
            else // if item doesn't exist in cart, create new record
            {
                sqlQuery = $"INSERT INTO cartitem (UserId, ProductId, Quantity) VALUES (\"{user.UserId}\", \"{addProductId}\", 1)";
            }
            rdr.Close();
            MySqlCommand insertCmd = new MySqlCommand(sqlQuery, conn);
            insertCmd.ExecuteNonQuery();

            conn.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        };
    }

    public static bool MergeCart(HttpResponse Response, HttpRequest http, string userId)
    {
        int quantity = 0;
        bool res = true;
        using (var conn = new MySqlConnection(data.cloudDB))
        {

            conn.Open();
            if (http.Cookies.Count() > 0)
            {

                foreach (KeyValuePair<string, string> c in http.Cookies)
                {
                    Console.WriteLine(c.Key);
                    Console.WriteLine(c.Value);

                    if (c.Key != "SessionId" && c.Key != "userID" && c.Key != "name" && c.Key != "username")
                    {
                        string checkIfProductExistsSql = $"SELECT Quantity FROM cartitem WHERE ProductId = \"{c.Key}\" and UserId =\"{userId}\"";
                        var cmd = new MySqlCommand(checkIfProductExistsSql, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            quantity = (int)reader[0];
                        }

                        string updateQuantitySql = "";
                        if (reader.HasRows)
                        {
                            // Insert the key value pair into the cartitem database
                            updateQuantitySql = $"UPDATE cartitem SET Quantity = \"{quantity}\" + \"{c.Value}\" WHERE ProductId = \"{c.Key}\" and UserId = \"{userId}\"";
                        }
                        else
                        {
                            // insert a new record into the table, where ProductId = {item.Key}, Quantity = {item.Value}
                            updateQuantitySql = $"INSERT INTO cartitem (UserId,ProductId, Quantity) VALUES (\"{userId}\",\"{c.Key}\",{c.Value}) ";
                        }

                        reader.Close();
                        var update = new MySqlCommand(updateQuantitySql, conn);
                        MySqlDataReader rdr = update.ExecuteReader();
                        Console.WriteLine(rdr.ToString());
                        res = rdr.RecordsAffected > 0 ? true : false;
                        rdr.Close();
                        Response.Cookies.Delete(c.Key);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            conn.Close();
            return res;
        }
    }

    public static Dictionary<Product, int> GetProductList(string userId)
    {

        Dictionary<Product, int> ProductList = new Dictionary<Product, int>();

        using (var connection = new MySqlConnection(data.cloudDB))
        {
            connection.Open();

            string sql = $"SELECT p.ProductId, p.Img, p.Name, p.Description, p.Price, c.Quantity FROM cartitem c, product p " +
                            $"WHERE c.ProductId = p.ProductId AND UserId = \"{userId}\"";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@userId", userId);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();
                product.ProductId = (string)reader["ProductId"];
                product.Img = (string)reader["Img"];
                product.Name = (string)reader["Name"];
                product.Description = (string)reader["Description"];
                product.Price = (int)reader["Price"];
                int ProductQty = (int)reader["Quantity"];

                ProductList.Add(product, ProductQty);
            }
            connection.Close();
        }
        return ProductList;
    }
}

