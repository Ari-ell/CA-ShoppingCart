using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Data;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;


namespace CA_ShoppingWebsite.Controllers;

public class CartController : Controller
{
    // GET: /<controller>/
    public IActionResult Index() {
        Dictionary<Product, int> ProductList = new Dictionary<Product, int>();

        string userId = Request.Cookies["userId"];
        if (userId != null)
        {
            ViewBag.userId = userId;

            ProductList = Data.CartItemData.GetProductList(userId);
            ViewBag.cartItems = ProductList;
        }
        //else {

        //    if (Request.Cookies.Count() > 0)
        //    {

        //        foreach (KeyValuePair<string, string> c in Request.Cookies)
        //        {
        //            Console.WriteLine(c.Key);
        //            Console.WriteLine(c.Value);

        //            if (c.Key != "SessionId" && c.Key != "userID" && c.Key != "name" && c.Key != "username")
        //            {
        //                ProductList.Add()


        //            }
        //        }
        //    }
        return View();
    }

    // from MouseClick
    // Ajax is calling this method

    // something like :
    //<input type = "button"
    //value="Go Somewhere Else" <-?
    //onclick="location.href='<%: Url.Action("AddProductToCart(productId)", "CartController") %>'" />
    [Route("addToCart")]
    public IActionResult AddProductToCart(string addProductId)
    {

        // Check if user is logged in
        User user = new User();
        string? userid = Request.Cookies["userID"];

        if (userid != null)
        {

            user.UserId = userid;

            // AddProductToCart Function
            // establish connection to DB
            MySqlConnection conn = new MySqlConnection(data.cloudDB);

            try
            {

                Console.WriteLine("Connecting to MySQL for Product Data...");
                conn.Open();

                // check if item exists in cart
                string querySql = @"SELECT * FROM cartItem WHERE cartItem.ProductId = @addProductId AND cartItem.UserId = @userId";
                MySqlCommand queryCmd = new MySqlCommand(querySql, conn);
                queryCmd.Parameters.AddWithValue("@addProductId", addProductId);
                queryCmd.Parameters.AddWithValue("@userId", user.UserId);
                MySqlDataReader rdr = queryCmd.ExecuteReader();
                string sqlQuery = "";
                // if item already exists in cart, first item in DB
                if (rdr.HasRows)
                {

                    Console.WriteLine("User is logged in. Product currently exists in Cart. Connecting to MySQL to write Product Data...");
                    sqlQuery = @"UPDATE cartitem SET quantity = quantity + 1 WHERE productId = @addProductId and UserId = @user.UserId";
                    //MySqlCommand updateCmd = new MySqlCommand(sqlQuery, conn);
                    // if item doesn't exist in cart, create new reccord

                }
                else
                {

                    Console.WriteLine("User is logged in. Product doesn't exist in Cart. Connecting to MySQL to write Product Data...");
                    sqlQuery = @"INSERT INTO cartitem (UserId,ProductId, Quantity) VALUES (@user.UserId, @addProductId, 1)";

                }
                rdr.Close();
                MySqlCommand insertCmd = new MySqlCommand(sqlQuery, conn);
                insertCmd.Parameters.AddWithValue("@user.UserId", @user.UserId);
                insertCmd.Parameters.AddWithValue("@addProductId", @addProductId);
                MySqlDataReader res = insertCmd.ExecuteReader();

                res.Close();
            
                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            };

        }
        else
        {
            string? cookieQuantity = Request.Cookies[addProductId];
            // check if product is present in cookies
            if (cookieQuantity != null)
            {

                int? productQuantity = Convert.ToInt32(cookieQuantity);
                // if yes, add 1 to value
                Response.Cookies.Append($"{addProductId}", $"{productQuantity + 1}");

            }
            else
            {

                // add product to Session Object Cart, set quantity to 1
                Response.Cookies.Append($"{addProductId}", "1");

            }

            
        }

        // Ok response to browser, not View()
        return Ok();

    }

    // [Start]
    // If user is logged in, will redirect to myPurchases view
    // If not logged in, will redirect to login page to log in
    // Upon succesfull log in, will re-direct back to Cart to checkout
    // If unsucessful, will catch and show exception msg
    // [Checkout procedure]
    // Once checkout is clicked, all CartItem products (for specified user)
    // will be wrapped in a PurchaseOrder object and
    // transferred to PurchaseOrder table
    // Update PurchaseList table with activation codes
    // After that, CartItem table will be cleared for that specific user
    // [End]

    // What happens if the cart has 0 items and user tries to checkout?
    public IActionResult Checkout()
    {
        // Check if user is logged in
        var getUserId = Request.Cookies["userId"];

        // Complete check out procedure and redirect to myPurchases
        if (getUserId != null)
        {
            Data.CartData.CheckOutUser(getUserId);
            return RedirectToAction("Index", "MyPurchases");
        }
        // Redirect to Login if not logged in
        // Need to redirect back to cart once logged in?
        else {
            return RedirectToAction("Index", "Login");
        }
    }

}

