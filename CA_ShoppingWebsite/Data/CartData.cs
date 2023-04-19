using System;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CA_ShoppingWebsite.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.AspNetCore.Mvc;

namespace CA_ShoppingWebsite.Data;

public class CartData
{
    // Perform all actions needed to checkout user
    // 1. Convert cart items into list of PO objects
    // 2. Add PO list iteratively to PO records
    // 3. Add into PList records with actvCode
    // 4. Clear cart items
    public static void CheckOutUser(string userId)
    {
        var poList = GetPoList(userId);

        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            MySqlTransaction trans = conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand("", conn, trans);

            try
            {
                // Add each PO into PurchaseOrder table as record
                foreach (var po in poList)
                {   
                    cmd.CommandText = $"INSERT INTO PurchaseOrder(PurchaseId, UserId, ProductId, PurchaseQty, PurchaseDate) " +
                                    $"VALUES (\"{po.PurchaseId}\", \"{userId}\", " +
                                    $"\"{po.ProductId}\", \"{po.PurchaseQty}\", \"{po.PurchaseDate}\")";
                    cmd.ExecuteNonQuery();

                    // Insert PurchaseId and ActvCode based on qty
                    for (int i = 0; i < po.PurchaseQty; i++)
                    {
                        var actvCode = Guid.NewGuid();
                        cmd.CommandText = $"INSERT INTO PurchaseList(PurchaseId, ActivationCode) VALUES(\"{po.PurchaseId}\", \"{actvCode.ToString()}\");";
                        cmd.ExecuteNonQuery();
                    }
                }
                // Clear CartItem records based on the userId
                cmd.CommandText = $"DELETE FROM CartItem WHERE UserId = \"{userId}\";";
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            // Rollback execution if there is an exception
            catch (Exception ex)
            {
                Debug.WriteLine("Somes error with DB: ", ex.Message);
                trans.Rollback();
            }
        }
    }

    // Get a list of POs based on matching userId
    public static List<Models.PurchaseOrder> GetPoList(string userId)
    {
        var poList = new List<Models.PurchaseOrder>();
        var curDate = GetCurrentDate();

        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            string sql = $"SELECT ProductId, Quantity FROM CartItem WHERE UserId = \"{userId}\"";
            var cmd = new MySqlCommand(sql, conn);

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var purchaseId = Guid.NewGuid();
                var po = new Models.PurchaseOrder
                {
                    PurchaseId = purchaseId.ToString(),
                    UserId = userId,
                    ProductId = (string)reader["ProductId"],
                    PurchaseQty = (int)reader["Quantity"],
                    PurchaseDate = curDate
                };
                poList.Add(po);
            }
            conn.Close();
        }
        return poList;
    }

    // Gets current date in day-month-year format
    public static string GetCurrentDate()
    {
        DateTime curDate = DateTime.Now;
        return curDate.ToString("dd MMM yyyy");
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

    public static List<Product> products()
    {
        List<Product> ProductList = new List<Product>();
        using (var connection = new MySqlConnection(data.cloudDB))
        {
            connection.Open();

            string sql = $"SELECT p.ProductId, p.Img, p.Name, p.Description, p.Price FROM product p";

            var cmd = new MySqlCommand(sql, connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();

                product.ProductId = (string)reader["ProductId"];
                product.Img = (string)reader["Img"];
                product.Name = (string)reader["Name"];
                product.Description = (string)reader["Description"];
                product.Price = (int)reader["Price"];

                ProductList.Add(product);
            }
            connection.Close();
        }
        return ProductList;
    }

    

}

