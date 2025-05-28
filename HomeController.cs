using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using econest.Models;
using Microsoft.AspNetCore.Mvc;

namespace econest.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Shweta\\Documents\\econestdb.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adp = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index()
        {
            List<tblproduct> productList = new List<tblproduct>();
            con.Open();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            adp = new SqlDataAdapter("SELECT pid,pname, pdes, price, material, image FROM tblproduct", con);
            adp.Fill(ds);
            dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                productList.Add(new tblproduct
                {
                    pid = Convert.ToInt32(dr["pid"].ToString()),
                    pname = dr["pname"].ToString(),
                    pdes = dr["pdes"].ToString(),
                    price = Convert.ToInt32(dr["price"].ToString()),
                    material = dr["material"].ToString(),
                    image = dr["image"].ToString()
                });
            }
            return View(productList); // Pass list to View
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

        public IActionResult aboutus()
        {
            return View();
        }

        public IActionResult contactus()
        {
            return View();
        }
    }
}
