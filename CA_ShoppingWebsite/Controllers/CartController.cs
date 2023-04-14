using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CA_ShoppingWebsite.Controllers;

public class CartController : Controller
{
    // GET: /<controller>/
    public IActionResult Index()
    {
        // Navbar with "View Cart" and Continue Shopping (Gallery) | Checkout (Purchase History)
        // Retrieve products from user's cart
        // Check if user isLoggedIn
            // if no:
            // retrieve from Session Object
                // display in card format on Cart Page
                // individual price display per product
                // display quantity of product
            // if yes:
            // retrieve from DB
                // display in card format on Cart Page
                // individual price display per product
                // display quantity of product
        // Total Price display
            // sum of products (calculate from DB (Query for Price) + Session Object)
            
        // Adjust Quantity Dropdown or +-
        // Check if user isLoggedIn
            // if no:
                // Session Object.Add(ProductId), just increment by 1
            // if yes:
                // UPDATE Quantity in CartItem Table, increment by 1

        return View();
    }

    // from MouseClick
    // Ajax is calling this method
    public IActionResult AddProductToCart(int newProductId)
    {
        // Check if user is logged in

        // if not logged in:
        // assuming Session Object is already created
        // store Product in Session Object
            // check if Product already exists in Session Object
            // if no:
                // HttpContext.Session.SetInt32("ProductId", 1)
            // if yes:
                // int Quantity = HttpContext.Session.GetInt("ProductId) + 1;
                // HttpContext.Session.SetInt32("ProductId", Quantity)

        // if logged in:
        // check if Product already exists in CartItem
            // if no:
                // INSERT, Quantity = 1
            // if yes:
                // UPDATE, Quantity += 1

        // send response to Gallery page
        // Gallery page updates the cart (somehow, AJAX?)

        // I don't think it returns a view
        return View();
    }


    // Checkout adds all products in CartItem table to PurchaseOrder table
    // If user is logged in, will redirect to myPurchases view
    // If not logged in, will redirect to login page to log in
    //      Upon succesfull log in, will re-direct back to Cart
    // If unsucessful, try catch show exception
    public IActionResult Checkout()
    {
        // Check if user is logged in
        var userId = (string)Request.Cookies["userId"];
        if (userId == null)
            return RedirectToAction("Index", "Login");
        else {
            //ConvertCartToPO();
            //AddPOToPurchases();
            return RedirectToAction("Index", "MyPurchases");
        }
    }



}

