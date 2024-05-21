using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Data;
using mygallery.Extensions;
using mygallery.Models;
using mygallery.Models.ViewModels;

namespace mygallery.Controllers
{
    [Authorize]
    public class TekliflerController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppConfig appConfig;

        public TekliflerController(ILogger<HomeController> logger, MyGalleryContext context, IOptions<AppConfig> config)
        {
            dbContext = context;
            _logger = logger;
            appConfig = config.Value;
        }

        [HttpGet]
        public IActionResult Liste()
        {
            PageInit("Teklif Listesi", "dashboard", "actions",
            new List<Breadcrumb>
            {
                new("Teklif Listesi", "/teklifler/liste")
            });

            return View();
        }

        [HttpPost]
        public JsonResult json_get_request_data([FromBody] RequestSearchData search)
        {
            if(search.sortBy == "")
                search.sortBy="Status,CreatedAt";
            
            var req=dbContext
            .BuyRequests
            .Where(x=>x.Model.BrandId==search.BrandId||search.BrandId==null)
            .Where(x=>x.ModelId==search.ModelId||search.ModelId==null)
            .Where(x=>x.Year==search.Year||search.Year==null)
            .Where(x=>($"{x.FistName} ${x.LastName}").Contains(search.Requester)||string.IsNullOrWhiteSpace(search.Requester))
            .Where(x=>x.IsCompleted==search.Status||search.Status==null)
            .OrderByDescending(x=>x.IsCompleted)
            .Select(x => new
            {
                RequestId = x.RequestId,
                BrandName = x.Model.Brand.BrandName,
                ModelName = x.Model.ModelName,
                Year = x.Year,
                GearType = x.GearType,
                FuelType = x.FuelType,
                Requester = x.FistName + " " + x.LastName,
                Status=x.IsCompleted?"Yanıtlandı":"Beklemede",
                CreatedAt=x.CreatedAt,
            })
            .ToGridJson(search.sortBy, search.skip, search.take);

            return Json(req);
        }


        [HttpGet]
        public IActionResult Detay(int id)
        {
            PageInit("Teklif Listesi", "dashboard", "actions",
            new List<Breadcrumb>
            {
                new("Teklif Listesi", "/teklifler/liste"),
                new("Teklif Detayı", $"/teklifler/{id}/detay"),
            });
            
            var request=dbContext
            .BuyRequests
            .Include(x=>x.Model)
            .Include(x=>x.Model.Brand)
            .Include(x=>x.BuyRequestExtensions)
            .Include(x=>x.RequestDamageInfos)
            .FirstOrDefault(x=>x.RequestId==id);

            if(request==null)
            {
                return View("Error",new ErrorViewModel{
                    PageTitle="Hata",
                    ErrorTitle="Teklif Bulunamadı",
                    ErrorDesc="Belirtilen talep silinmiş olabilir. Geri gelip sayfayı yenile ve sonrasında tekrar teklife git."
                });
            }

            var vm=new TekliflerDetayViewModel(){
                BrandName=request.Model.Brand.BrandName,
                CarInfo=request.RequestDamageInfos
                .Select(x=>new TekliflerDetayViewModel.CarPartInfo{
                    Part=x.PartName,
                    Status=x.Damage
                }).ToList(),
                CreatedAt=request.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                Extensions=request.BuyRequestExtensions.Select(x=>x.Extension.ExtensionName).ToList(),
                FirstName=request.FistName,
                LastName=request.LastName,
                FuelType=request.FuelType,
                GearType=request.GearType,
                Infos=request.ExtraExtension,
                IsCompleted=request.IsCompleted,
                ModelName=request.Model.ModelName,
                Km=request.Km,
                PhoneNo=request.GsmNo.StartsWith("5")?$"+90{request.GsmNo}":request.GsmNo.StartsWith("0")?$"+9{request.GsmNo}":request.GsmNo,
                Price=request.Price,
                RequestId=request.RequestId,
                Year=request.Year
            };

            return View(vm);
        }

        [HttpPost]
        public JsonResult json_respond_request([FromBody] IntDecimalParam data){
            var request=dbContext
            .BuyRequests
            .Where(x=>!x.IsCompleted)
            .Where(x=>x.RequestId==data.Id)
            .FirstOrDefault();

            if(request==null){
                return Json(new Result(false,"Talep yanıtlanmış veya silinmiş, lütfen sayfayı yenileyerek kontrol ediniz."));
            }
            request.Price=data.Value;
            request.IsCompleted=true;
            dbContext.SaveChanges();
            return Json(new Result(true,"Teklif kaydedildi. Whtasappa yönlendiriliyorsunuz..."));
        }
    }
}