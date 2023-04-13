using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using Google.Protobuf.Compiler;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers;

public class GalleryController : Controller
{
    // GET: /<controller>/
    public IActionResult Index(User? user,string? keyword)
    {
        ViewBag.users = user;
        string userid = Request.Cookies["userID"];
        string username = Request.Cookies["username"];
        string name = Request.Cookies["name"];
        user.Name = name;
        user.UserId = Convert.ToInt32( userid);
        user.Username = username;
        var products = Data.ProductData.GetAllProducts();
        ViewBag.products = Search(keyword, products);
        return View();
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

