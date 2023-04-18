using System;
using System.Runtime.CompilerServices;
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

    // Returns a dictionary with key as the purchase Id and
	// value being a list of activation codes for the key 
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
}

