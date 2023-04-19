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
                List<Product> products = Data.CartData.products();

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
            MySqlConnection conn = new MySqlConnection(data.cloudDB);
            try
            {
                conn.Open();

                // check if item exists in cart
                string querySql = $"UPDATE cartitem SET quantity = {qty} WHERE productId = \"{productID}\" and UserId = \"{userID}\"";
                MySqlCommand queryCmd = new MySqlCommand(querySql, conn);
                MySqlDataReader rdr = queryCmd.ExecuteReader();
                conn.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        else
        {
            string? cookieQuantity = Request.Cookies[productID];
            // check if product is present in cookies
            if (cookieQuantity != null)
            {
                int? productQuantity = Convert.ToInt32(cookieQuantity);
                // if yes, add 1 to value
                Response.Cookies.Append($"{productID}", $"{qty}");
            }
            else
            {
                // add product to Session Object Cart, set quantity to 1
                Response.Cookies.Append($"{productID}", "1");
            }
        }
        return Ok();
    }


    // from MouseClick
    // Ajax is calling this method

    // something like :
    //<input type = "button"
    //value="Go Somewhere Else" <-?
    //onclick="location.href='<%: Url.Action("AddProductToCart(productId)", "CartController") %>'" />
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
            // establish connection to DB
            MySqlConnection conn = new MySqlConnection(data.cloudDB);

            try
            {
                //Console.WriteLine("Connecting to MySQL for Product Data...");
                conn.Open();

                // check if item exists in cart
                string querySql = $"SELECT * FROM cartItem WHERE cartItem.ProductId = \"{addProductId}\" AND cartItem.UserId = \"{user.UserId}\"";
                MySqlCommand queryCmd = new MySqlCommand(querySql, conn);
                MySqlDataReader rdr = queryCmd.ExecuteReader();
                string sqlQuery = "";

                if (rdr.HasRows) // if item already exists in cart, update record quantity
                {

                    Console.WriteLine("User is logged in. Product currently exists in Cart. Connecting to MySQL to write Product Data...");
                    sqlQuery = $"UPDATE cartitem SET quantity = quantity + 1 WHERE productId = \"{addProductId}\" and UserId = \"{user.UserId}\"";

                }
                else // if item doesn't exist in cart, create new record
                {
                    Console.WriteLine("User is logged in. Product doesn't exist in Cart. Connecting to MySQL to write Product Data...");
                    sqlQuery = $"INSERT INTO cartitem (UserId, ProductId, Quantity) VALUES (\"{user.UserId}\", \"{addProductId}\", 1)";
                }
                rdr.Close();
                MySqlCommand insertCmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader res = insertCmd.ExecuteReader();

                res.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            };
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

