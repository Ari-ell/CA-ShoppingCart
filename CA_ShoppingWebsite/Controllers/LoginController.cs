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
using System.Net;
using System.Xml;

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
        // Convert JSON data
        Request.Headers.TryGetValue("username",out var usernameObj);
        Request.Headers.TryGetValue("password", out var passwordObj);
        string username = usernameObj.ToString();
        string password = passwordObj.ToString();

        bool hasMerged = false;

        User user = new Models.User();

        // Try to match the login input to users db
        if (username != null && password != null) {
             user = UserData.GetUserLogin(username, password);
        }

        if (user.UserId == null)
        {
            return Unauthorized();
        }
        else {
            if (Request.Cookies["SessionId"] == null) {

                string sessionId = System.Guid.NewGuid().ToString();
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("SessionId", sessionId, options);
            }

            Response.Cookies.Append("userID", user.UserId);
            Response.Cookies.Append("username", user.Username);
            Response.Cookies.Append("name", user.Name);
    
    
            hasMerged = Data.CartData.MergeCart(Response, this.Request, user.UserId); // Need to pass in the Http request
        }
        if (hasMerged)
        {
            return Ok(user);
        }
        else {
            return BadRequest(user);
        }
    }

    [Route("logout")]
    public IActionResult Logout() {
        
        Response.Cookies.Delete("userID");
        Response.Cookies.Delete("username");
        Response.Cookies.Delete("name");
        Response.Cookies.Delete("SessionId");

        return Ok();
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

