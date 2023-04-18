using MySql.Data.MySqlClient;
using System;
namespace CA_ShoppingWebsite.Data
{
	public class ReviewData
	{
		// Get user reviews for individual products
		// Store in a dictionary w key-value pairs of produt-review
		public static Dictionary<int,int>? GetUserReviews(string userId)
		{
			var userReviews = new Dictionary<int, int>();


			return userReviews;
		}

		// Set the user review value into review db
		// and update avg rating to product db
		public static void SetReviews()
		{

		}

        public static int GetProductRating(string productId)
        {

            int averageRating = 0;
            using (var conn = new MySqlConnection(data.cloudDB))
            {

                conn.Open();

                string sql = @"SELECT AVG(Rating) FROM review WHERE ProductId= @productId;";

                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@productId", productId);

                averageRating = Convert.ToInt32((cmd.ExecuteScalar()==DBNull.Value ? null : cmd.ExecuteScalar()));

                conn.Close();
            }

            return averageRating;
        }
    }
}

