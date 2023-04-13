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
        List<Product> products = getProducts();
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
                if (product.Name.Contains(keyword,
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    selected.Add(product);
                }
            }
        }

        return selected;

    }
    public List<Product> getProducts()
    {

        List<Product> products = new List<Product>();
        products.Add(new Product
        {
            Name = "ABC",
     
            Price = 3,
            Img = "iceCream.jpeg",
            Description = " this is the description for product ABC",

        });

        products.Add(new Product
        {
            Name = "123",

            Price = 4.5,
            Img = "iceCream.jpeg",
            Description = " this is the description for product 123",

        });

        products.Add(new Product
        {
            Name = "456",
        
            Price = 5.7,
            Img = "iceCream.jpeg",
            Description = " this is the description for product 456",

        });
        products.Add(new Product
        {
            Name = "TEST 124",
            Price = 6.8,
            Img = "iceCream.jpeg",
            Description = " this is the description for product 456",

        });
        products.Add(new Product
        {
            Name = "TEST 124",
            Price = 2.7,
            Img = "iceCream.jpeg",
            Description = " this is the description for product 456",

        });
        products.Add(new Product
        {
            Name = "TEST 124",
            Price = 9.0,
            Img = "iceCream.jpeg",
            Description = " this is the description for product 456",

        });
        return products;


    }

}

