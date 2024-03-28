using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mygallery.Context;
using mygallery.Models;
using System.IO;
using Newtonsoft.Json;
using mygallery.Infrastuctures;
using Microsoft.VisualBasic;
using mygallery.Data;
using mygallery.Models.ViewModels;

namespace mygallery.Controllers;
[Route("[action]")]
public class HomeController : BaseController
{
	private readonly ILogger<HomeController> _logger;
	private readonly LoginHelper loginHelper;

	public HomeController(ILogger<HomeController> logger, MyGalleryContext context)
	{
		dbContext = context;
		_logger = logger;
		loginHelper=new LoginHelper(
			context,appConfig,dataProtector,"mg_co","",180
		);
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
	public IActionResult Giris(string returnUrl)
	{
		var cookie = loginHelper.GetCookie(HttpContext);
		var vm=new LoginViewModel{
			HasError=false,
			ErrorDesc="",
			LoginName=cookie.LoginName,
			LoginPassword=cookie.LoginPassword,
			RememberMe=cookie.RememberMe,
			ReturnUrl=returnUrl
		};
		return View(vm);
	}

	[HttpPost]
	public async Task<IActionResult> Giriş( [FromBody] LoginData data){
		
		var vm=new LoginViewModel{
			HasError=false,
			ErrorDesc="Lütfen kullanıcı adı ve şifre giriniz.",
			LoginName=data.LoginName,
			LoginPassword=data.LoginPassword,
			RememberMe=data.RememberMe,
			ReturnUrl=data.ReturnUrl
		};

		if (string.IsNullOrWhiteSpace(data.LoginName) || string.IsNullOrWhiteSpace(data.LoginPassword)) 
			return View(vm);

		var result=await loginHelper.LoginAsync(HttpContext,data.LoginName,data.LoginPassword,data.RememberMe);
		if(result.Status){
			return Redirect(string.IsNullOrWhiteSpace(data.ReturnUrl)?"/yonetim/anasayfa":data.ReturnUrl);
		}else{
			vm.ErrorDesc=result.Message;
		}

		return View(vm);
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
