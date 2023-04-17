using System;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CA_ShoppingWebsite.Data;

public class CartData
{
    // Perform all actions needed to checkout
    // 1. Convert cart items into list of PO objects
    // 2. Add PO list iteratively to PO records
    // 3. Add into PList records with actvCode
    // 4. Clear cart items
    public static void CheckOutUser(int userId)
    {
        Console.WriteLine("Starting checkout..");

        // Get list of POs
        var poList = GetPoList(userId);
        Console.WriteLine("Purchase Order List retrieved..");

        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            Console.WriteLine("Connecting to cloudDB..");
            MySqlTransaction trans = conn.BeginTransaction();
            MySqlCommand cmd = new MySqlCommand("", conn, trans);
            try
            {
                foreach (var po in poList)
                {
                    Console.WriteLine("Adding record into PO table");
                    // Add PO list into PurchaseOrder table as records
                    cmd.CommandText = @"INSERT INTO PurchaseOrder
                                        VALUES (@PurchaseId, @UserId, @ProductId, @PurchaseQty, @PurchaseDate);";
                    cmd.Parameters.AddWithValue("@PurchaseId", po.PurchaseId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", po.ProductId);
                    cmd.Parameters.AddWithValue("@PurchaseQty", po.PurchaseQty);
                    cmd.Parameters.AddWithValue("@PurchaseDate", po.PurchaseQty);

                    cmd.ExecuteNonQuery();

                    // Insert into PurchaseList based on PurchaseId and qty
                    // with ActvCodes based on qty
                    Console.WriteLine("Adding record into PList table");
                    for (int i = 0; i < po.PurchaseQty; i++)
                    {
                        var actvCode = new Guid();
                        cmd.CommandText = @"INSERT INTO PurchaseList
                                            VALUES(@Purchase, @ActivationCode);";
                        cmd.Parameters.AddWithValue("@PurchaseId",po.PurchaseId);
                        cmd.Parameters.AddWithValue("@ActivationCode", actvCode);

                        cmd.ExecuteNonQuery();
                    }
                    Console.WriteLine("Adding Purchase records into table - COMPLETE");
                }
                // Clear CartItem records
                Console.WriteLine("Clearing cart items");
                cmd.CommandText = @"DELETE FROM CartItem
                                    WHERE UserId = @userId;";
                cmd.Parameters.AddWithValue("@userId", userId);

                cmd.ExecuteNonQuery();

                Console.WriteLine("User cart emptied. Checkout completed");

                trans.Commit();
            }
            // Roll back the execution if encounnter exception
            catch (Exception ex)
            {
                Debug.WriteLine("Somes error with DB: ", ex.Message);
                trans.Rollback();
            }
        }
    }


    // Get a list of POs based on matching
    // the userId to a cartId
    public static List<Models.PurchaseOrder> GetPoList(int userId)
    {
        var poList = new List<Models.PurchaseOrder>();
        var curDate = GetCurrentDate();

        using (var conn = new MySqlConnection(data.cloudDB))
        {
            conn.Open();
            string sql = @"SELECT ProductId, Quantity 
                        FROM CartItem
                        WHERE UserId = @userId;";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@userId", userId);

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var purchaseId = new Guid();
                var po = new Models.PurchaseOrder
                {
                    PurchaseId = purchaseId.ToString(),
                    UserId = userId,
                    ProductId = (int)reader["ProductId"],
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

}    

