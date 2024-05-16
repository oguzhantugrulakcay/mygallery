using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mygallery.Context;
using mygallery.Models;
using Newtonsoft.Json;
using mygallery.Infrastuctures;
using mygallery.Data;
using mygallery.Models.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;

namespace mygallery.Controllers;
[Route("[action]")]
public class HomeController : BaseController
{
	private readonly ILogger<HomeController> _logger;
	private readonly LoginHelper loginHelper;
	private readonly AppConfig appConfig;
	private readonly IDataProtectionProvider dataProtectionProvider;
	public HomeController(ILogger<HomeController> logger, MyGalleryContext context,IOptions<AppConfig> config,IDataProtectionProvider dataProtectionProvider)
	{
		dbContext = context;
		_logger = logger;
		appConfig=config.Value;
		this.dataProtectionProvider=dataProtectionProvider;
		loginHelper=new LoginHelper(
			context,appConfig,dataProtectionProvider,"mg_co","",180
		);
	}
	[HttpGet("/")]
	public IActionResult Index()
	{
		return View();
	}
	#region SatimAlma
	[HttpGet]
	public IActionResult SatinAlma()
	{
		var vm=new SatinAlmaViewModel{
			Extensions=dbContext
			.CarExtensions
			.Select(e=>new SatinAlmaViewModel.Extension{
				ExtensionId=e.ExtensionId,
				ExtensionName=e.ExtensionName
			}).ToList()
		};
		return View(vm);
	}

	[HttpPost]
	public JsonResult json_send_request([FromBody] CustomerBuyRequestData data){
		string errorMessage="Beklenmeyen bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.";
		var justRequest=new BuyRequest{
				FistName=data.FirstName,
				LastName=data.LastName,
				CreatedAt=DateTime.Now,
				ExtraExtension=data.Infos,
				FuelType=data.FuelType,
				GearType=data.GearType,
				GsmNo=data.PhoneNo,
				ModelId=data.ModelId,
				Year=data.Year,

		};

		dbContext.BuyRequests.Add(justRequest);
		try
		{
		dbContext.SaveChanges();
		}
		catch (Exception)
		{
			return Json(new Result(false,errorMessage));
		}
		foreach(var info in data.CarInfo){
				var justInfo=new RequestDamageInfo{
					RequestId=justRequest.RequestId,
					PartName=info.Part,
					Damage=info.Status,
				};
				dbContext.RequestDamageInfos.Add(justInfo);
		}

		try
		{
		dbContext.SaveChanges();
		}
		catch (System.Exception)
		{			
			return Json(new Result(false,errorMessage));
		}
		foreach(var extension in data.ExtensionIds){
			var justExtension=new BuyRequestExtension{
				ExtensionId=extension,
				IsHave=true,
				RequestId=justRequest.RequestId
			};
			dbContext.BuyRequestExtensions.Add(justExtension);
		}

		try
		{
		dbContext.SaveChanges();
		}
		catch (System.Exception)
		{			
			return Json(new Result(false,errorMessage));
		}
//SendMail

		return Json(new Result(true,"Talebiniz alınmıştır. En kısa sürede size dönüş yapıyor olacağız."));
	}


	
#endregion
	[HttpGet]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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
	public async Task<IActionResult> Giris(string LoginName, string LoginPassword, string RememberMe, string ReturnUrl){

		var vm=new LoginViewModel{
			HasError=true,
			ErrorDesc="Lütfen kullanıcı adı ve şifre giriniz.",
			LoginName=LoginName,
			LoginPassword=LoginPassword,
			RememberMe=RememberMe,
			ReturnUrl=ReturnUrl
		};

		if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(LoginPassword)) 
			return View(vm);

		var result=await loginHelper.LoginAsync(HttpContext,LoginName,LoginPassword,RememberMe);
		if(result.Status){
			var redirectUrl=string.IsNullOrWhiteSpace(ReturnUrl)?"/yonetim/anasayfa":ReturnUrl;
			return RedirectToAction("Anasayfa","Yonetim");
		}else{
			vm.ErrorDesc=result.Message;
		}

		return View(vm);
	}

	[HttpGet]
	[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Cikis(){
		loginHelper.Logout(HttpContext);
		return RedirectToAction("Giris");
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
