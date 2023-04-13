using System;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Data;

public class PurchaseData
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
						WHERE UserId = " + userId;

			var cmd = new MySqlCommand(sql, conn);
			MySqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				var purchase = new Models.PurchaseOrder() {
					PurchaseId = (Guid)reader["PurchaseId"],
					ProductId = (int)reader["ProductId"],
					PurchaseQty = (int)reader["PurchaseQty"],
					PurchaseDate = (string)reader["PurchaseDate"]
				};
				myPurchases.Add(purchase);
			}
		}
		return myPurchases;
	}

	public static List<Models.PurchaseList>  GetActivationCodes()
	{
		var actvCodes = new List<Models.PurchaseList>();


		return actvCodes;
    }
}

