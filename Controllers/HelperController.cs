using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Models;

namespace mygallery.Controllers
{
    public class HelperController : BaseController
    {
        public HelperController(MyGalleryContext context, IOptions<AppConfig> appConfig, IWebHostEnvironment env)
        {
            hostingEnvironment = env;
            dbContext = context;
            this.appConfig = appConfig.Value;
        }
        [HttpGet]
        public JsonResult getBrands()
        {
            var brands = dbContext.Brands.Select(b => new
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName
            }).ToList();
            return Json(brands);
        }

        [HttpGet]
        public JsonResult getModels(int? BrandId)
        {
            var models = dbContext.Models
                .Where(m => m.BrandId == BrandId)
                .Select(m => new
                {
                    ModelId = m.ModelId,
                    ModelName = m.ModelName
                }).ToList();
            return Json(models);
        }
    }
}
