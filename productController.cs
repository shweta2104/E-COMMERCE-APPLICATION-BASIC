using econest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Data;
using System.Data.SqlClient;

namespace econest.Controllers
{
    public class productController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataSet ds= new DataSet();
        DataTable dt = new DataTable();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult proin()
        {
            dropdownlist();
            return View();
        }

        [HttpPost]
        public IActionResult proin(tblproduct p,IFormFile file)
        {
            string query;
            con.Open();
            upload(file);
            cmd = new SqlCommand("insert into tblproduct (cid, pname, pdes, price, material, image) values(" + p.cid + ", '" + p.pname + "', '" + p.pdes + "', " + p.price + ", '" + p.material + "', '" + file.FileName.ToString() + "')", con);
            cmd.ExecuteNonQuery();
            return View();
        }

        public async Task<IActionResult> upload(IFormFile file)
        {
            if (file != null)
            {
                string path = Path.Combine("wwwroot/uploads", file.FileName);
                TempData["filename"] = file.FileName;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return View();
        }

        public void dropdownlist()
        {
            var clist = new List<tblcat>();
            adp = new SqlDataAdapter("select cid,cname from tblcat", con);
            adp.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                clist.Add(new tblcat
                {
                    cid = Convert.ToInt32(dr["cid"].ToString()),
                    cname = dr["cname"].ToString()
                });
            }

            ViewBag.clist = clist.Select(d => new SelectListItem
            {
                Value = d.cid.ToString(),
                Text = d.cname.ToString()
            }).ToList();
        }

        public IActionResult product()
        {
            con.Open();
            var plist = new List<tblproduct>();
            //adp = new SqlDataAdapter("SELECT c.cname AS CategoryName,p.pname AS ProductName,p.price,p.material,p.[image ] AS Image FROM dbo.tblproduct p LEFT JOIN dbo.tblcat c ON p.cid = c.cid", con);
            adp = new SqlDataAdapter("select pid,pname,pdes,price,material,image from tblproduct where cid=1", con);
            adp.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                plist.Add(new tblproduct
                {
                    pid = Convert.ToInt32(dr["pid"]),
                    pname = dr["pname"].ToString(),
                    pdes = dr["pdes"].ToString(),
                    price = Convert.ToInt32(dr["price"].ToString()),
                    material = dr["material"].ToString(),
                    image = dr["image"].ToString()
                });
            }
            return View(plist);
        }

        public IActionResult product1()
        {
            con.Open();
            var plist = new List<tblproduct>();
            //adp = new SqlDataAdapter("SELECT c.cname AS CategoryName,p.pname AS ProductName,p.price,p.material,p.[image ] AS Image FROM dbo.tblproduct p LEFT JOIN dbo.tblcat c ON p.cid = c.cid", con);
            adp = new SqlDataAdapter("select pid,pname,pdes,price,material,image from tblproduct where cid=2", con);
            adp.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                plist.Add(new tblproduct
                {
                    pid = Convert.ToInt32(dr["pid"]),
                    pname = dr["pname"].ToString(),
                    pdes = dr["pdes"].ToString(),
                    price = Convert.ToInt32(dr["price"].ToString()),
                    material = dr["material"].ToString(),
                    image = dr["image"].ToString()
                });
            }
            return View(plist);
        }

        public IActionResult product2()
        {
            con.Open();
            var plist = new List<tblproduct>();
            //adp = new SqlDataAdapter("SELECT c.cname AS CategoryName,p.pname AS ProductName,p.price,p.material,p.[image ] AS Image FROM dbo.tblproduct p LEFT JOIN dbo.tblcat c ON p.cid = c.cid", con);
            adp = new SqlDataAdapter("select pid,pname,pdes,price,material,image from tblproduct where cid=3", con);
            adp.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                plist.Add(new tblproduct
                {
                    pid = Convert.ToInt32(dr["pid"]),
                    pname = dr["pname"].ToString(),
                    pdes = dr["pdes"].ToString(),
                    price = Convert.ToInt32(dr["price"].ToString()),
                    material = dr["material"].ToString(),
                    image = dr["image"].ToString()
                });
            }
            return View(plist);
        }

        public IActionResult product3()
        {
            con.Open();
            var plist = new List<tblproduct>();
            //adp = new SqlDataAdapter("SELECT c.cname AS CategoryName,p.pname AS ProductName,p.price,p.material,p.[image ] AS Image FROM dbo.tblproduct p LEFT JOIN dbo.tblcat c ON p.cid = c.cid", con);
            adp = new SqlDataAdapter("select pid,pname,pdes,price,material,image from tblproduct where cid=4", con);
            adp.Fill(ds);
            dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                plist.Add(new tblproduct
                {
                    pid = Convert.ToInt32(dr["pid"]),
                    pname = dr["pname"].ToString(),
                    pdes = dr["pdes"].ToString(),
                    price = Convert.ToInt32(dr["price"].ToString()),
                    material = dr["material"].ToString(),
                    image = dr["image"].ToString()
                });
            }
            return View(plist);
        }
        //public IActionResult cartitem()
        //{
        //    con.Open();
        //    var clist = new List<tblcart>();
        //    adp = new SqlDataAdapter("select pname,pdes,price,material,image,qty,created_at from tblcart", con);
        //    adp.Fill(ds);
        //    dt = ds.Tables[0];

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        clist.Add(new tblcart
        //        {
        //            pname = dr["pname"].ToString(),
        //            pdes = dr["pdes"].ToString(),
        //            price = Convert.ToInt32(dr["price"].ToString()),
        //            material = dr["material"].ToString(),
        //            image = dr["image"].ToString(),
        //            qty = Convert.ToInt32(dr["qty"].ToString()),
        //            created_at = Convert.ToDateTime(dr["created_at"].ToString())

        //        });
        //    }
        //    return View(clist);
        //}
    }
}
