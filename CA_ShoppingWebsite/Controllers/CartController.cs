using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Data;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;


namespace CA_ShoppingWebsite.Controllers;

public class CartController : Controller
{
    // GET: /<controller>/
    public IActionResult Index() {
        Dictionary<Product, int> ProductList = new Dictionary<Product, int>();

        string userId = Request.Cookies["userId"];
        if (userId != null)
        {
            ViewBag.userId = userId;

            ProductList = Data.CartData.GetProductList(userId);
            ViewBag.cartItem = ProductList;
        }
        else
        {
            if (Request.Cookies.Count() > 0)
            {
                List<Product> products = Data.ProductData.GetAllProducts();

                foreach (KeyValuePair<string, string> c in Request.Cookies)
                {
                    Console.WriteLine(c.Key);
                    Console.WriteLine(c.Value);

                    if (c.Key != "SessionId" && c.Key != "userID" && c.Key != "name" && c.Key != "username")
                    {
                        foreach (var product in products)
                        {

                            if (product.ProductId == c.Key)
                            {
                                ProductList.Add(product, Convert.ToInt32(c.Value));
                            }
                        }
                    }
                }
                ViewBag.cartItem = ProductList;
            }
        }
      
        return View();
    }

    [Route("editQty")]
    public IActionResult editQty(int? qty, string? productID)
    {
        string userID = Request.Cookies["userID"];
        if (userID != null)
        {
            Data.CartData.EditCartQty(userID, productID, qty);
        }
        else
        {
            string? cookieQuantity = Request.Cookies[productID];

            if (qty > 0)
            {
                int? productQuantity = Convert.ToInt32(cookieQuantity);

                Response.Cookies.Append($"{productID}", $"{qty}");
            }
            else {
                Response.Cookies.Delete($"{productID}");
            }
      
        }
        return Ok();
    }


    // JSON pass the cart info to this method
    // to update product and qty for user
    // based on either cookie or db
    [Route("addToCart")]
    public IActionResult AddProductToCart(string addProductId)
    {

        // Check if user is logged in
        User user = new User();
        string? userid = Request.Cookies["userID"];

        if (userid != null)
        {
            user.UserId = userid;

            // AddProductToCart Function
            Data.CartData.AddProductToCart(user, addProductId);
        }
        else
        {
            string? cookieQuantity = Request.Cookies[addProductId];
            // check if product is present in cookies
            if (cookieQuantity != null)
            {
                int? productQuantity = Convert.ToInt32(cookieQuantity);
                // if yes, add 1 to value
                Response.Cookies.Append($"{addProductId}", $"{productQuantity + 1}");
            }
            else
            {
                // add product to Session Object Cart, set quantity to 1
                Response.Cookies.Append($"{addProductId}", "1");
            }
        }
        // Ok response to browser, not View()
        return Ok();
    } 
}

