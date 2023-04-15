using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Data;
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


    // [Start]
    // If user is logged in, will redirect to myPurchases view
    // If not logged in, will redirect to login page to log in
    //      Upon succesfull log in, will re-direct back to Cart to checkout
    // If unsucessful, will catch and show exception msg
    // [Checkout procedure]
    // Once checkout is clicked, all CartItem products (for specified user)
    // will be wrapped in a PurchaseOrder object and
    // transferred to PurchaseOrder table
    // Update PurchaseList table with activation codes
    // After that, CartItem table will be cleared for that specific user
    // [End]
    public IActionResult Checkout()
    {
        // Check if user is logged in
        var getUserId = Request.Cookies["userId"];

        // Redirect to Login if not logged in
        // Need to redirect back to cart once logged in?
        if (getUserId == null)
            return RedirectToAction("Index", "Login");

        // Complete check out procedure and redirect to myPurchases
        else {
            var userId = Convert.ToInt32(getUserId);
            Data.CartData.CheckOutUser(userId);

            return RedirectToAction("Index", "MyPurchases");
        }
    }



}

