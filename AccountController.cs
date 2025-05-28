using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using econest.Models;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace econest.Controllers
{
    public class AccountController : Controller
    {

        private readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30";

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(tbluser u)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO tbluser (uname, email, password, role) VALUES (@uname, @email, @password, @role)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uname", u.uname);
                        cmd.Parameters.AddWithValue("@email", u.email);
                        cmd.Parameters.AddWithValue("@password", u.password);
                        cmd.Parameters.AddWithValue("@role", u.role);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["message"] = "Registration successful!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["message"] = "Error: " + ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //public IActionResult Login(string uname1, string email1, string pass)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            string query = "SELECT * FROM tbluser WHERE uname = @uname AND email = @email AND password = @password";
        //            using (SqlCommand cmd = new SqlCommand(query, con))
        //            {
        //                cmd.Parameters.AddWithValue("@uname", uname1);
        //                cmd.Parameters.AddWithValue("@email", email1);
        //                cmd.Parameters.AddWithValue("@password", pass);

        //                con.Open();
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        // Save user info to session
        //                        HttpContext.Session.SetString("uid", reader["uid"].ToString());
        //                        HttpContext.Session.SetString("username", reader["uname"].ToString());
        //                        HttpContext.Session.SetString("email", reader["email"].ToString());
        //                        HttpContext.Session.SetString("role", reader["role"].ToString());

        //                        TempData["status"] = "true";

        //                        // Redirect based on role
        //                        string role = reader["role"].ToString().ToLower();
        //                        if (role == "customer")
        //                        {
        //                            return RedirectToAction("Index", "Home");
        //                        }
        //                        else if (role == "supplier")
        //                        {
        //                            return RedirectToAction("Dashboard", "supplier");
        //                        }
        //                        else
        //                        {
        //                            return RedirectToAction("Index", "Home");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TempData["status"] = "false";
        //                        ViewBag.Error = "Invalid login credentials.";
        //                        return View();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["status"] = "false";
        //        ViewBag.Error = "Error: " + ex.Message;
        //        return View();
        //    }
        //}

        public IActionResult Login(string uname1, string email1, string pass)
        {
            if (string.IsNullOrWhiteSpace(uname1) || string.IsNullOrWhiteSpace(email1) || string.IsNullOrWhiteSpace(pass))
            {
                ViewBag.Error = "Please fill in all fields.";
                TempData["status"] = "false";
                return View();
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM tbluser WHERE uname = @uname AND email = @email AND password = @password";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@uname", uname1);
                        cmd.Parameters.AddWithValue("@email", email1);
                        cmd.Parameters.AddWithValue("@password", pass);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Set authentication cookies
                                CookieOptions options = new CookieOptions
                                {
                                    Expires = DateTimeOffset.Now.AddDays(30),
                                    HttpOnly = true
                                };

                                Response.Cookies.Append("uid", reader["uid"].ToString(), options);
                                Response.Cookies.Append("username", reader["uname"].ToString(), options);
                                Response.Cookies.Append("email", reader["email"].ToString(), options);
                                Response.Cookies.Append("role", reader["role"].ToString(), options);

                                TempData["status"] = "true";

                                // Redirect based on user role
                                string role = reader["role"].ToString().ToLower();
                                if (role == "customer")
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                                else if (role == "supplier")
                                {
                                    return RedirectToAction("Dashboard", "Supplier");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            else
                            {
                                TempData["status"] = "false";
                                ViewBag.Error = "Invalid login credentials.";
                                return View();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["status"] = "false";
                ViewBag.Error = "Error: " + ex.Message;
                return View();
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("uid");
            Response.Cookies.Delete("username");
            Response.Cookies.Delete("email");
            Response.Cookies.Delete("role");

            return RedirectToAction("Login", "Account");
        }

    }
}
