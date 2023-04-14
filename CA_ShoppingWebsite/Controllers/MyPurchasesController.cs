using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers;

public class PurchasesController : Controller
{
    public IActionResult Index()
    {
        // When user accesses the Purchases Controller
        // Will see all past purchases and be able to set reviews
        string checkUser = Request.Cookies["userId"];
        if (checkUser != null)
        {
            // Returns the userID whose MyPurchases page will be loaded
            var userId = Convert.ToInt32(checkUser);
            ViewBag.userId = userId;

            //
            var myPurchases = Data.PurchaseData.GetPurchaseOrders(userId);
            ViewBag.myPurchases = myPurchases;

            var myActivationCodes = Data.PurchaseData.GetActivationCodes(userId);
            ViewBag.myActivationCodes = myActivationCodes;

            var myProducts = Data.ProductData.GetProductDetails(userId);
            ViewBag.myProducts = myProducts;

            var myReviews = Data.ReviewData.GetUserReviews(userId);
            ViewBag.myReviews = myReviews;

            // If user edits or adds a review
            //      SetReviews();
            return View();
        }
        else
            return RedirectToAction("Index", "Gallery");
    }
}

