using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CA_ShoppingWebsite.Models;
using MySql.Data.MySqlClient;

namespace CA_ShoppingWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        Search();
        return View();
    }
    [Route("search")]
    public User Search() {
        User user = new User();
        string connStr = "server=2016110.lfidc.net;user=mysql7402439;database=mysql7402439_db;port=3306;password=soLjM9NJzO;Connect Timeout=60";
        MySqlConnection conn = new MySqlConnection(connStr);
      
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

