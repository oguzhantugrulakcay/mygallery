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
            //todo: ToGridJson not work
            var req=dbContext.BuyRequests.Select(x => new
            {
                RequestId = x.RequestId,
                BrandName = x.Model.Brand.BrandName,
                ModelName = x.Model.ModelName,
                Year = x.Year,
                GearType = x.GearType,
                FuelType = x.FuelType,
                Requester = x.FistName + " " + x.LastName
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