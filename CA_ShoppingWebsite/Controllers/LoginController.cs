using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index()
        {

            // Search();
            return View();
        }

        [HttpGet("userlogin")]
        public ActionResult ExtractFromBasic()
        {
            Request.Headers.TryGetValue("username",out var usernameObj);
            Request.Headers.TryGetValue("password", out var passwordObj);
            string username = usernameObj.ToString();
            string password = passwordObj.ToString();
            User user = new Models.User();
            if (username != null && password != null) {
                 user = userLogin(username, password);

            }
            if (user.ID == null) {

                return Unauthorized();
            }
            return Ok(user);
        }

        public User userLogin(string username,string password)
        {
          
            User user = new User();
            MySqlConnection conn = new MySqlConnection(data.cloudDB);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM user WHERE Username =@username and Password =@password";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.ID = rdr[0].ToString();
                    user.Username = rdr[1].ToString();
                    user.Name = rdr[3].ToString();
                    user.History = null;
                    user.Cart = null;
                  
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");
            return user;

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

