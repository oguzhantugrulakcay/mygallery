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
using Microsoft.EntityFrameworkCore;

namespace mygallery.Controllers;
[Route("[action]")]
public class HomeController : BaseController
{
	private readonly ILogger<HomeController> _logger;
	private readonly LoginHelper loginHelper;
	private readonly AppConfig appConfig;
	private readonly IDataProtectionProvider dataProtectionProvider;
	private readonly IMailService mailService;
	public HomeController(ILogger<HomeController> logger, MyGalleryContext context,IOptions<AppConfig> config,IDataProtectionProvider dataProtectionProvider,IMailService _mailService)
	{
		dbContext = context;
		_logger = logger;
		appConfig=config.Value;
		this.dataProtectionProvider=dataProtectionProvider;
		loginHelper=new LoginHelper(
			context,appConfig,dataProtectionProvider,"mg_co","",180
		);
		mailService=_mailService;
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
	public async Task<JsonResult> json_send_request([FromBody] CustomerBuyRequestData data){
		string errorMessage="Beklenmeyen bir sorun oluştu. Lütfen daha sonra tekrar deneyiniz.";
		
		var justRequest=new BuyRequest{
			CreatedAt=DateTime.Now,
			FistName=data.FirstName,
			FuelType=data.FuelType,
			GearType=data.GearType,
			GsmNo=data.PhoneNo,
			ExtraExtension=data.Infos,
			IsCompleted=false,
			LastName=data.LastName,
			Year=data.Year,
			ModelId=data.ModelId,
			Km=data.Km,
		};
		await dbContext.BuyRequests.AddAsync(justRequest);
		
		try
		{
			await dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			
			#if DEBUG
			return Json(new Result(false,ex.Message+ex.InnerException?.Message));
			#else
			return Json(new Result(false,errorMessage))
			#endif

		}
		
		foreach(var info in data.CarInfo){
				var justInfo=new RequestDamageInfo{
					RequestId=justRequest.RequestId,
					PartName=info.Part,
					Damage=info.Status,
				};
				await dbContext.RequestDamageInfos.AddAsync(justInfo);
		}

		try
		{
		dbContext.SaveChanges();
		}
		catch (System.Exception ex)
		{		
			#if DEBUG
			return Json(new Result(false,ex.Message+ex.InnerException?.Message));
			#else
			return Json(new Result(false,errorMessage))
			#endif
		}
		
		foreach(var extension in data.ExtensionIds){
			var justExtension=new BuyRequestExtension{
				ExtensionId=extension,
				IsHave=true,
				RequestId=justRequest.RequestId
			};
			await dbContext.BuyRequestExtensions.AddAsync(justExtension);
		}

		try
		{
		await dbContext.SaveChangesAsync();
		}
		catch (Exception ex)
		{			
			#if DEBUG
			return Json(new Result(false,ex.Message+ex.InnerException?.Message));
			#else
			return Json(new Result(false,errorMessage))
			#endif
		}
		
		var request=await dbContext.BuyRequests.Include(r=>r.Model).FirstOrDefaultAsync(r=>r.RequestId==justRequest.RequestId);

		try{
			await mailService.SendBuyRequestMail(request.Model.ModelName,request.Model.Brand.BrandName,request.Year,request.RequestId);
			return Json(new Result(true,"Talebiniz alınmıştır. En kısa sürede size dönüş yapıyor olacağız."));
		}catch(Exception ex){
			#if DEBUG
			return Json(new Result(false,ex.Message+ex.InnerException?.Message));
			#else
			return Json(new Result(false,errorMessage))
			#endif
		}
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

	

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel());
	}
}
