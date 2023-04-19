using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Data;
using CA_ShoppingWebsite.Models;
using Google.Protobuf.Compiler;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Controllers;

public class GalleryController : Controller
{
    // GET: /<controller>/
    public IActionResult Index(User? user, string? keyword, int? cartQty)
    {
        // Get user info if available
        ViewBag.users = user;

        // Create a new session and add into cookies
        string sessionId = System.Guid.NewGuid().ToString();
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Append("SessionId", sessionId, options);

        // Get user details from cookies
        string userId = Request.Cookies["userID"];
        string username = Request.Cookies["username"];
        string name = Request.Cookies["name"];
        user.Name = name;
        user.UserId = userId;
        user.Username = username;

        // Get product details to display in gallery
        var products = Data.ProductData.GetAllProducts();
        

        ViewBag.products = Data.GalleryData.Search(keyword, products!);
        ViewBag.cartQty = Data.GalleryData.checkQty(this.Request, userId);

        return View();
    }
}

