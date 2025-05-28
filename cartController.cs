using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using econest.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleCart.Controllers
{
    public class CartController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();

        public IActionResult AddToCart(tblcart c)
        {
            try
            {
                string userIdStr = Request.Cookies["uid"];
                if (string.IsNullOrEmpty(userIdStr))
                {
                    TempData["msg"] = "User not logged in. Cannot add to cart.";
                    return RedirectToAction("cartitem");
                }
                int userId = Convert.ToInt32(userIdStr);

                using (SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30"))
                {
                    con.Open();

                    // Verify product exists and get its info if needed (optional step)
                    SqlCommand productCmd = new SqlCommand("SELECT COUNT(*) FROM tblproduct WHERE pid = @pid", con);
                    productCmd.Parameters.AddWithValue("@pid", c.pid);

                    int productCount = (int)productCmd.ExecuteScalar();

                    if (c.pid <= 0)
                    {
                        TempData["msg"] = "Invalid product ID.";
                        return RedirectToAction("cartitem");
                    }


                    // Try to fetch existing cart item for this user and product
                    SqlCommand fetchCartCmd = new SqlCommand("SELECT cid, qty FROM tblcart WHERE pid = @pid AND uid = @uid", con);
                    fetchCartCmd.Parameters.AddWithValue("@pid", c.pid);
                    fetchCartCmd.Parameters.AddWithValue("@uid", userId);

                    using (SqlDataReader cartReader = fetchCartCmd.ExecuteReader())
                    {
                        if (cartReader.Read())
                        {
                            // Entry exists: get cart id and existing qty
                            int cid = Convert.ToInt32(cartReader["cid"]);
                            int existingQty = Convert.ToInt32(cartReader["qty"]);
                            cartReader.Close();

                        }
                        else
                        {
                            cartReader.Close();

                            // Entry not found: insert new
                            SqlCommand insertCmd = new SqlCommand("INSERT INTO tblcart (pid, uid, qty, created_at) VALUES (@pid, @uid, @qty, @created_at)", con);
                            insertCmd.Parameters.AddWithValue("@pid", c.pid);
                            insertCmd.Parameters.AddWithValue("@uid", userId);
                            insertCmd.Parameters.AddWithValue("@qty", c.qty);
                            insertCmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                            insertCmd.ExecuteNonQuery();

                            TempData["msg"] = "Product added to cart.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "An error occurred: " + ex.Message;
                // You might want to log the exception as well here
            }

            return RedirectToAction("cartitem");
        }
        public IActionResult cartitem()
        {
            string userIdStr = Request.Cookies["uid"];
            var clist = new List<prolist>();
            decimal total = 0;

            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["msg"] = "User not logged in.";
                ViewBag.Total = total;
                return View("cartItem", clist);
            }

            int userId = Convert.ToInt32(userIdStr);

            using (SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                string query = @"
            SELECT 
                p.pid, p.cid, p.pname, p.pdes, p.price, p.material, p.image, 
                c.qty, c.created_at
            FROM tblcart c
            INNER JOIN tblproduct p ON p.pid = c.pid
            WHERE c.uid = @uid";

                SqlDataAdapter adp = new SqlDataAdapter(query, con);
                adp.SelectCommand.Parameters.AddWithValue("@uid", userId);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var item = new prolist
                    {
                        pid = Convert.ToInt32(dr["pid"]),
                        cid = Convert.ToInt32(dr["cid"]),
                        pname = dr["pname"].ToString(),
                        pdes = dr["pdes"].ToString(),
                        price = Convert.ToInt32(dr["price"]),
                        material = dr["material"].ToString(),
                        image = dr["image"].ToString(),
                        qty = Convert.ToInt32(dr["qty"]),
                        created_at = Convert.ToDateTime(dr["created_at"])
                    };

                    clist.Add(item);

                    // Add to total
                    total += item.price * item.qty;
                }
            }

            ViewBag.Total = total; // ✅ Important: Pass total to view
            return View("cartItem", clist);
        }

        //Remove Product
        [HttpPost]
        public IActionResult Remove(int productId)
        {
            string userIdStr = Request.Cookies["uid"];
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["msg"] = "User not logged in.";
                return RedirectToAction("cartitem");
            }

            int userId = Convert.ToInt32(userIdStr);

            using (SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tblcart WHERE pid = @pid AND uid = @uid", con);
                cmd.Parameters.AddWithValue("@pid", productId);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.ExecuteNonQuery();
            }

            TempData["msg"] = "Item removed from cart.";
            return RedirectToAction("cartitem");
        }

        //Update Quantity
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            string userIdStr = Request.Cookies["uid"];
            if (string.IsNullOrEmpty(userIdStr))
            {
                TempData["msg"] = "User not logged in.";
                return RedirectToAction("cartitem");
            }

            int userId = Convert.ToInt32(userIdStr);

            using (SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblcart SET qty = @qty WHERE pid = @pid AND uid = @uid", con);
                cmd.Parameters.AddWithValue("@qty", quantity);
                cmd.Parameters.AddWithValue("@pid", productId);
                cmd.Parameters.AddWithValue("@uid", userId);

                cmd.ExecuteNonQuery();
            }

            TempData["msg"] = "Cart updated successfully.";
            return RedirectToAction("cartitem");
        }

       
























































































        //old code

        //public IActionResult AddToCart(tblcart c)
        //{
        //    con.Open();

        //    // Check if the product exists in tblproduct
        //    SqlCommand checkProductCmd = new SqlCommand("SELECT COUNT(*) FROM tblproduct WHERE pid = @pid", con);
        //    checkProductCmd.Parameters.AddWithValue("@pid", c.pid);
        //    int productExists = (int)checkProductCmd.ExecuteScalar();

        //    if (productExists == 0)
        //    {
        //        // If product does not exist, display a message
        //        TempData["msg"] = "Product not found. Cannot add to cart.";
        //        con.Close();
        //        return RedirectToAction("cartItem");
        //    }

        //    // Insert the cart item
        //    cmd = new SqlCommand("INSERT INTO tblcart (pid, qty, created_at) VALUES (@pid, @qty, @created_at)", con);
        //    cmd.Parameters.AddWithValue("@pid", c.pid);
        //    cmd.Parameters.AddWithValue("@qty", c.qty);
        //    cmd.Parameters.AddWithValue("@created_at", DateTime.Now);

        //    cmd.ExecuteNonQuery();
        //    con.Close();

        //    TempData["msg"] = "Product added to cart.";
        //    return RedirectToAction("cartitem");
        //}


        //old code

        //public IActionResult cartitem()
        //{
        //    con.Open();
        //    var clist = new List<prolist>();
        //    adp = new SqlDataAdapter(@"
        //    SELECT 
        //        tblproduct.pid, tblproduct.cid, tblproduct.pname, tblproduct.pdes, 
        //        tblproduct.price, tblproduct.material, tblproduct.image, 
        //        tblcart.qty, tblcart.created_at
        //    FROM tblcart 
        //    INNER JOIN tblproduct ON tblproduct.pid = tblcart.pid", con);

        //    adp.Fill(ds);
        //    dt = ds.Tables[0];

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        clist.Add(new prolist
        //        {
        //            pid = Convert.ToInt32(dr["pid"]),
        //            cid = Convert.ToInt32(dr["cid"]),
        //            //uid = Convert.ToInt32(dr["uid"]),
        //            pname = dr["pname"].ToString(),
        //            pdes = dr["pdes"].ToString(),
        //            price = Convert.ToInt32(dr["price"]),
        //            material = dr["material"].ToString(),
        //            image = dr["image"].ToString(),
        //            qty = Convert.ToInt32(dr["qty"]),
        //            created_at = Convert.ToDateTime(dr["created_at"])
        //        });
        //    }

        //    con.Close();
        //    return View("cartItem", clist);

        //}



    }


}

