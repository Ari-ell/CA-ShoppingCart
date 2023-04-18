using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers;

public class MyPurchasesController : Controller
{
    public IActionResult Index()
    {
        // When user accesses the Purchases Controller
        // Will see all past purchases and be able to set reviews
        string checkUser = Request.Cookies["userId"];
        if (checkUser != null)
        {
            // Returns the userID whose MyPurchases page will be loaded
            var userId = checkUser;

            // Gets user purchase history
            var myPurchases = Data.MyPurchaseData.GetPurchaseOrders(userId);
            ViewBag.myPurchases = myPurchases;

            // Gets the activations codes for each purchaseId
            var myActivationCodes = Data.MyPurchaseData.GetActivationCodes(userId);
            ViewBag.myActivationCodes = myActivationCodes;

            // Gets product details to be populated onto MyPurchases
            var myProducts = Data.ProductData.GetProductDetails(userId);
            ViewBag.myProducts = myProducts;

            var myReviews = Data.ReviewData.GetUserReviews(userId);
            ViewBag.myReviews = myReviews;

            return View();
        }
        else
            return RedirectToAction("Index", "Gallery");
    }

    // Need to add method to update user reviews
    // This method should also update the overall user reviews

}

