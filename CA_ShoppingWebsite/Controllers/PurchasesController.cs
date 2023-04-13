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
        string checkUser = Request.Cookies["userId"];
        if (checkUser != null)
        {
            var userId = Convert.ToInt32(checkUser);
            var myPurchases = Data.PurchaseData.GetPurchaseOrders(userId);
            var myActivationCodes = Data.PurchaseData.GetActivationCodes(userId);
            var myProducts = Data.ProductData.GetProductDetails(userId);
            var myReviews = Data.ReviewData.GetUserReviews(userId);



            // If user edits or adds a review
            //      SetReviews();
        }
        // When user accesses the Purchases Controller
        // Will see all past purchases and be able to set reviews
        return View();
    }
}

