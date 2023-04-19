using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CA_ShoppingWebsite.Models;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Data;

public class MyPurchaseData
{
	/// <summary>
	// Once purchases are entered into database
	// The user is already logged in and purchase
	// is considered compelete.
	///
	/// In MyPurchases page, user will be able to see all
	/// their past purchases sorted by Date of Purchase
	/// and product with qty and activation code combo box
	/// </summary>

	public static List<Models.PurchaseOrder> GetPurchaseOrders(string userId)
	{
		var myPurchases = new List<Models.PurchaseOrder>();
		using (var conn = new MySqlConnection(data.cloudDB))
		{
            conn.Open();
            string sql = @"SELECT * from PurchaseOrder
						WHERE UserId = @userId;";

			var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@userId", userId);
			MySqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				var purchase = new Models.PurchaseOrder() {
					UserId = userId,
					PurchaseId = (string)reader["PurchaseId"],
					ProductId = (string)reader["ProductId"],
					PurchaseQty = (int)reader["PurchaseQty"],
					PurchaseDate = (string)reader["PurchaseDate"]
				};
				myPurchases.Add(purchase);
			}
			conn.Close();
		}
		return myPurchases;
	}

    // Returns a dictionary with purchase Id as key and
    // list of activation codes as value
    public static Dictionary<string, List<string>> GetActivationCodes(string userId)
	{
		var actvCodes = new Dictionary<string, List<string>>();

        using (var conn = new MySqlConnection(data.cloudDB))
		{
			conn.Open();
			string sql = @"SELECT PurchaseId, ActivationCode
						FROM PurchaseList
						WHERE PurchaseList.PurchaseId IN
						(SELECT PurchaseOrder.PurchaseId
						FROM PurchaseOrder
						WHERE UserId = @userId);";

			var cmd = new MySqlCommand(sql, conn);
			cmd.Parameters.AddWithValue("@userId", userId);

			MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
				var ActivationCode = (string)reader["ActivationCode"];
				var PurchaseId = (string)reader["PurchaseId"];
				if (actvCodes.ContainsKey(PurchaseId))
				{
					actvCodes[PurchaseId].Add(ActivationCode);
				}
				else
				{
					actvCodes[PurchaseId] = new List<string>();
                    actvCodes[PurchaseId].Add(ActivationCode);
                }
			}
			conn.Close();
        }
		return actvCodes;
    }

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

    
}

