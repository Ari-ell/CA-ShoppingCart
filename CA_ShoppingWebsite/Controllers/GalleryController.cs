using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Data;
using CA_ShoppingWebsite.Models;
using Google.Protobuf.Compiler;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers;

public class GalleryController : Controller
{
    // GET: /<controller>/
    public IActionResult Index(User? user,string? keyword,int? carQty)
    {
        ViewBag.users = user;

        string sessionId = System.Guid.NewGuid().ToString();
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Append("SessionId", sessionId, options);

        string userid = Request.Cookies["userID"];
        string username = Request.Cookies["username"];
        string name = Request.Cookies["name"];
        user.Name = name;
        user.UserId = userid;
        user.Username = username;
        var products = Data.ProductData.GetAllProducts();
        
        ViewBag.products = Search(keyword, products!);
       
        ViewBag.carQty = checkQty(this.Request,userid);

        return View();
    }

    // need to read and understand
    public static int checkQty(HttpRequest request, string userId) {

        int cartCounter = 0;
        if (userId != null) {
            //Count Qty
            MySqlConnection conn = new MySqlConnection(data.cloudDB);
            conn.Open();
            string countQuery = $"SELECT SUM(Quantity) FROM cartItem WHERE UserId = \"{userId}\"";
            MySqlCommand countQty = new MySqlCommand(countQuery, conn);
            MySqlDataReader resQty = countQty.ExecuteReader();
            resQty.Read();
            cartCounter = Convert.ToInt32(resQty[0]);
            conn.Close();
        }
        else {
            if (request.Cookies.Count() > 0)
            {
                foreach (KeyValuePair<string, string> c in request.Cookies)
                {
                    if (c.Key != "SessionId" && c.Key != "userID" && c.Key != "name" && c.Key != "username")
                    {
                        cartCounter += Convert.ToInt32(c.Value);
                    }
                }
            }
        }
        return cartCounter;
    }


    public List<Product> Search(string keyword, List<Product> products)
    {
        if (keyword == "" || keyword == null) {
            return products;
        }
        List<Product> selected = new List<Product>();

        foreach (Product product in products)
        {
            if (product.Name != null)
            {
                if (product.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) ||
                    product.Description.Contains(keyword,StringComparison.CurrentCultureIgnoreCase))
                {
                    selected.Add(product);
                }
            }
        }
        return selected;
    }
}

