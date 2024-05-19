using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Data;
using mygallery.Extensions;
using mygallery.Models;

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
            return View();
        }

        [HttpPost]
        public JsonResult json_get_request_data([FromBody] RequestSearchData search)
        {
            if(search.sortBy == "")
                search.sortBy="CreatedAt";
            
            var req=dbContext
            .BuyRequests
            .Where(x=>x.Model.BrandId==search.BrandId||search.BrandId==null)
            .Where(x=>x.ModelId==search.ModelId||search.ModelId==null)
            .Where(x=>x.Year==search.Year||search.Year==null)
            .Where(x=>($"{x.FistName} ${x.LastName}").Contains(search.Requester)||string.IsNullOrWhiteSpace(search.Requester))
            .Where(x=>x.IsCompleted==search.Status||search.Status==null)
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

            return View();
        }
    }
}