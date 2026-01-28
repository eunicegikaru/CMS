using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    //public IActionResult TestDb()
    //{
    //    string message;

    //    try
    //    {
    //        var connStr = _configuration.GetConnectionString("DefaultConnection");

    //        using var conn = new SqlConnection(connStr);
    //        conn.Open();

    //        message = "? Database connected successfully!";
    //    }
    //    catch (Exception ex)
    //    {
    //        message = "? Connection failed: " + ex.Message;
    //    }

    //    // Pass message to the view
    //    ViewBag.DbStatus = message;
    //    return View();
    //}
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }   
}
