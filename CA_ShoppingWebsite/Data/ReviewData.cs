using System;
namespace CA_ShoppingWebsite.Data
{
	public class ReviewData
	{
		// Get user rviews for individual products
		// Store in a dictionary w key-value pairs of produt-review
		public static Dictionary<int,int>? GetUserReviews(int userId)
		{
			var userReviews = new Dictionary<int, int>();


			return userReviews;
		}

		// Set the user review value into review db
		// and update avg rating to product db
		public static void SetReviews()
		{

		}
	}
}

