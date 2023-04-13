using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
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

        }
        return Ok(user);
    }



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

