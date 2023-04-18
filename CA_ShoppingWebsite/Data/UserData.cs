using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_ShoppingWebsite.Controllers;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

// Use this to query db for user related info

namespace CA_ShoppingWebsite.Data;

public class UserData
{
    public static Models.User? GetUserInfo(string? username, string? password)
    {

        return null;
    }


    public static Models.User GetUserLogin(string username, string password)
    {

        var user = new Models.User();
        MySqlConnection conn = new MySqlConnection(data.cloudDB);

        try
        {
            //Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            string sql = $"SELECT * FROM user WHERE Username =\"{username}\" and Password =\"{password}\"";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            //cmd.Parameters.AddWithValue("@username", username);
            //cmd.Parameters.AddWithValue("@password", password);

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                user.UserId = (string)rdr[0];
                user.Username = rdr[1].ToString();
                user.Name = rdr[3].ToString();
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        conn.Close();
        //Console.WriteLine("Done.");
        return user;
    }
}

