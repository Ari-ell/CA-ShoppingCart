using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using CA_ShoppingWebsite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers;

public class LoginController : Controller
{

    public IActionResult Index()
    {

        // Search();
        return View();
    }

    [HttpGet("userlogin")]
    public ActionResult ExtractFromBasic()
    {
        Request.Headers.TryGetValue("username",out var usernameObj);
        Request.Headers.TryGetValue("password", out var passwordObj);
        string username = usernameObj.ToString();
        string password = passwordObj.ToString();
        //string url = "/gallery";
        User user = new Models.User();
        if (username != null && password != null) {
             user = UserData.GetUserLogin(username, password);
        }
        if (user.UserId == null)
        {

            return Unauthorized();
        }
        else {
            Console.WriteLine("Done.");

            string sessionId = System.Guid.NewGuid().ToString();
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("userID", user.UserId.ToString());
            Response.Cookies.Append("username", user.Username);
            Response.Cookies.Append("name", user.Name);
            Response.Cookies.Append("SessionId", sessionId, options);
        }
        return Ok(user);
    }

    // Check if the login userid matches the cookie options
        //if (user.UserId == options.UserId)
        //{

    [Route("logout")]
    public IActionResult Logout() {
//    // Check if the login userid matches the cookie options
//        if (user.UserId == options.UserId)
//        {

        return Ok();
    }


//        //HttpContext.Session.SetInt32("ProductId", 3);

        // Call the mergecart function
            //MergeCart();

// Define the mergecart function
//        // Call the mergecart function
//            MergeCart();
//}

//// Define the mergecart function
//public void MergeCart()
//{

//    var sessionObjectCartItems = new Dictionary<string, int>
//            { {"A001", 3 },
//                {"A002" ,5} };

//    HttpContext.Session.SetDictionary<string, int>("PreloginCartItems", sessionObjectCartItems);


//    using (var conn = new MySqlConnection(data.cloudDB))
//    {

//        conn.Open();
//        // Loop through each key value pair in the session object
//        foreach (var item in sessionObjectCartItems)
//        {

//            // open a new connection to the DB

//            string checkIfProductExistsSql = $"SELECT * FROM cartItems WHERE cartItems.ProductId = {item.Key}";
//            var cmd = new MySqlCommand(checkIfProductExistsSql, conn);
//            MySqlDataReader reader = cmd.ExecuteReader();

//            // if above SQL query returns true, i.e., product exists in cartItems DB
//            if (reader.GetBoolean(0))
//            {

//                // Insert the key value pair into the cartitem database
//                string updateQuantitySql = $"UPDATE cartItems SET Quantity = Quantity + {item.Value} WHERE cartItems.ProductId = {item.Key}";

//            {

//                // insert a new record into the table, where ProductId = {item.Key}, Quantity = {item.Value}
//                string insertProductSql = $"INSERT INTO cartItems (ProductId, Quantity) VALUES ({item.Key},{item.Value}) ";

//            }
//        }
//    }


//    // Save changes to the database <- feroz said it's not needed but idk
//    //_dbContext.SaveChanges();

//    // Clear the session object
//    HttpContext.Session.Clear();
//}


public IActionResult Privacy()
{
    return View();
}

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
}

// current flow, once user & pw is ok, cookie is passed to client
// and client is redirected to home/gallery page
//
// somewhere bteween cookie and redirection, we check that the user is logged in
// if options.UserID != null
// then MergeCart()

// MergeCart()
// Foreach (var KeyValuePair in SessionObject) {
// INSERT cartID, KeyValuePair.String, KeyValuePair.Key into DB
// }
// Clear the Prelogin Cart (Session Object)
// HttpContext.Session.Clear()