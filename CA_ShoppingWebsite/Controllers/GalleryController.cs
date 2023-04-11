using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers
{
    public class GalleryController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.products = getProducts();
            return View();
        }


        public List<Products> getProducts()
        {

            List<Products> products = new List<Products>();
            products.Add(new Products
            {
                Name = "ABC",
                Title = "This is Product ABC",
                Price = 12.44,
                Img = "",
                Description = " this is the description for product ABC",

            });

            products.Add(new Products
            {
                Name = "123",
                Title = "This is Product 123",
                Price = 12.44,
                Img = "",
                Description = " this is the description for product 123",

            });

            products.Add(new Products
            {
                Name = "456",
                Title = "This is Product 456",
                Price = 12.44,
                Img = "",
                Description = " this is the description for product 456",

            });

            return products;


        }

    }
}

