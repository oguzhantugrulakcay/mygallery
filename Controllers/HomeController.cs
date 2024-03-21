using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mygallery.Context;
using mygallery.Models;
using System.IO;
using Newtonsoft.Json;

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

	[HttpGet]
	public IActionResult Satis()
	{
		return View();
	}

	[HttpPost]
	public JsonResult jsonLoadBranModels()
	{
		var datas = new List<BrandModelData>();
		if (System.IO.File.Exists("car_brand_models.json"))
		{
			var data = System.IO.File.ReadAllText("car_brand_models.json");
			datas = JsonConvert.DeserializeObject<List<BrandModelData>>(data);
		}

		foreach (var data in datas)
		{
			var justBrand = new Brand
			{
				BrandName = data.brand
			};

			dbContext.Brands.Add(justBrand);
			dbContext.SaveChanges();

			foreach (var model in data.models)
			{
				var justModel = new Model
				{
					BrandId = justBrand.BrandId,
					ModelName = model
				};
				dbContext.Models.Add(justModel);
				dbContext.SaveChanges();
			}
		}

		return Json(null);
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
