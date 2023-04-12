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
        if (user.UserId == null) {

            return Unauthorized();
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

