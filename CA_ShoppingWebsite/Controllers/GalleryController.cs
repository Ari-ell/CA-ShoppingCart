using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers;

public class GalleryController : Controller
{
    // GET: /<controller>/
    public IActionResult Index()
    {
        ViewBag.products = getProducts();
        return View();
    }


    public List<Product> getProducts()
    {

        List<Product> products = new List<Product>();
        products.Add(new Product
        {
            Name = "ABC",
     
            Price = 3,
            Img = "",
            Description = " this is the description for product ABC",

        });

        products.Add(new Product
        {
            Name = "123",

            Price = 4.5,
            Img = "",
            Description = " this is the description for product 123",

        });

        products.Add(new Product
        {
            Name = "456",
        
            Price = 5.7,
            Img = "",
            Description = " this is the description for product 456",

        });
        products.Add(new Product
        {
            Name = "TEST 124",
            Price = 6.8,
            Img = "",
            Description = " this is the description for product 456",

        });
        products.Add(new Product
        {
            Name = "TEST 124",
            Price = 2.7,
            Img = "",
            Description = " this is the description for product 456",

        });
        products.Add(new Product
        {
            Name = "TEST 124",
            Price = 9.0,
            Img = "",
            Description = " this is the description for product 456",

        });
        return products;


    }

}

