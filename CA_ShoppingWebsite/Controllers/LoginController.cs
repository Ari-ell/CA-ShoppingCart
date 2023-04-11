using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CA_ShoppingWebsite.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index()
        {

            ViewBag.users = mockUsers();
            Search();
            return View();
        }
        [Route("search")]
        // We need to add more stuff here

        public List<User> mockUsers() {
            List<User> users = new List<User>() { };

            users.Add(new User
            {
                Username = "Test",
                ID = "1",
                Name = "Feroz",
                Cart =null,
                History=null

            });


            return users;
        }
        public User Search()
        {
            User user = new User();
            MySqlConnection conn = new MySqlConnection(data.cloudDB);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM wp_users";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
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

