using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mygallery.Context;
using mygallery.Models;

namespace mygallery.Controllers;
[Route("[action]")]
public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyGalleryContext context)
    {
        dbContext = context;
        _logger = logger;
    }
    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult SatinAlma()
    {
        var cars = dbContext
            .Brands.ToList();
        return View();
    }

    [HttpGet]
    public IActionResult Giris()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
