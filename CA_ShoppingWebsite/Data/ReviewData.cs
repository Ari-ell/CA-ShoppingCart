using CA_ShoppingWebsite.Models;
using MySql.Data.MySqlClient;
using System;
namespace CA_ShoppingWebsite.Data
{
    public class ReviewData
    {
        //Set new records for new products and set initial rating to null
        public static void SetInitialRating(string userId)
        {
            using (var conn = new MySqlConnection(data.cloudDB))
            {
                conn.Open();
                string sql = $"SELECT DISTINCT PurchaseOrder.ProductId " +
                 $"FROM PurchaseOrder, Review  WHERE PurchaseOrder.userId = \"{userId}\"  " +
                 $"AND PurchaseOrder.ProductId NOT IN " +
                 $"(SELECT ProductId  FROM Review WHERE UserId = \"{userId}\");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmd.CommandText = $"INSERT INTO Review VALUES (@userId, @productId, @rating)";
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("productId", (string)reader["ProductId"]);
                    cmd.Parameters.AddWithValue("@rating", null);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }


        //To display the star rating in Mypurchases page based on userId and ProductId
        public static Dictionary<string, int> GetRating(string userId)
        {
            var ratingValue = new Dictionary<string, int>();

            using (var conn = new MySqlConnection(data.cloudDB))
            {
                conn.Open();
                string sql = @"SELECT ProductId, Rating FROM review WHERE UserId= @UserId;";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {


                    var ProductId = (string)reader["ProductId"];
                    var Rating = (reader["Rating"] as int?) ?? 0;
                    ratingValue.Add(ProductId, Rating);

                }
                conn.Close();
                return ratingValue;
            }
        }
        // Set the user review value into review db
        // and update avg rating to product db
        public static void SetRating(string userId, string productId, int userRating)
        {
            using (var conn = new MySqlConnection(data.cloudDB))
            {
                conn.Open();

                MySqlTransaction trans = conn.BeginTransaction();
                var cmd = new MySqlCommand("", conn, trans);



                cmd.CommandText = @"Update review SET Rating = @Rating WHERE UserId = @UserId AND ProductId =@ProductId";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Rating", userRating);

                cmd.ExecuteNonQuery();

                trans.Commit();

                conn.Close();
            }


        }


        //Average product rating for gallery page
        public static int GetProductRating(string productId)
        {

            int averageRating = 0;
            using (var conn = new MySqlConnection(data.cloudDB))
            {

                conn.Open();

                string sql = @"SELECT AVG(Rating) FROM review WHERE ProductId= @productId";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@productId", productId);

                averageRating = Convert.ToInt32((cmd.ExecuteScalar()==DBNull.Value ? null : cmd.ExecuteScalar()));

                conn.Close();
            }

            return averageRating;
        }
    }
}


