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
    
            hasMerged= MergeCart(user.UserId); // Need to pass in the Http request
        }
        if (hasMerged)
        {
            return Ok(user);
        }
        else {
            return BadRequest(user);
        }
    }

    // Need to move this to cartData.
    // but need to be able to pass in the HttpRequest
    public bool MergeCart(string userId)
    {
        int quantity = 0;
        bool res = true;
        using (var conn = new MySqlConnection(data.cloudDB))
        {

            conn.Open();
            if (Request.Cookies.Count() > 0)
            {

                foreach (KeyValuePair<string, string> c in Request.Cookies)
                {
                    Console.WriteLine(c.Key);
                    Console.WriteLine(c.Value);

                    if (c.Key != "SessionId" && c.Key != "userID" && c.Key != "name" && c.Key != "username")
                    {
                        string checkIfProductExistsSql = $"SELECT Quantity FROM cartitem WHERE ProductId = \"{c.Key}\" and UserId =\"{userId}\"";
                        var cmd = new MySqlCommand(checkIfProductExistsSql, conn);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            quantity = (int)reader[0];
                        }

                        string updateQuantitySql = "";
                        if (reader.HasRows)
                        {
                            // Insert the key value pair into the cartitem database
                            updateQuantitySql = $"UPDATE cartitem SET Quantity = \"{quantity}\" + \"{c.Value}\" WHERE ProductId = \"{c.Key}\" and UserId = \"{userId}\"";
                        }
                        else
                        {
                            // insert a new record into the table, where ProductId = {item.Key}, Quantity = {item.Value}
                            updateQuantitySql = $"INSERT INTO cartitem (UserId,ProductId, Quantity) VALUES (\"{userId}\",\"{c.Key}\",{c.Value}) ";
                        }

                        reader.Close();
                        var update = new MySqlCommand(updateQuantitySql, conn);
                        MySqlDataReader rdr = update.ExecuteReader();
                        Console.WriteLine(rdr.ToString());
                        if (rdr.RecordsAffected > 0)
                        {
                            res = true;
                        }
                        else
                        {
                            res = false;
                        }
                        rdr.Close();
                        Response.Cookies.Delete(c.Key);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            conn.Close();
            return res;
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

