using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Data;
using mygallery.Extensions;
using mygallery.Models.ViewModels;

namespace mygallery.Controllers
{

    [Authorize]
    public class YonetimController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppConfig appConfig;
        public YonetimController(ILogger<HomeController> logger, MyGalleryContext context, IOptions<AppConfig> config)
        {
            dbContext = context;
            _logger = logger;
            appConfig = config.Value;
        }

        #region Anasayfa

        [HttpGet]
        public IActionResult Anasayfa()
        {
            var currentUser = User.GetUser<LogonUser>();
            PageInit("Anasayfa", "dashboard", "actions",
            new List<Breadcrumb>
            {
                new("Anasayfa", "/yonetim/Anasayfa")
            });
            return View();
        }
        #endregion
        #region  Tanımlar
        [HttpGet]
        public IActionResult tanimlar()
        {
            PageInit("Tanımlar", "definitions", "actions",
            new List<Breadcrumb>{
                new("Anasayfa","/yonetim/anasayfa"),
                new("Tanımlar","/yonetim/tanimlar")
            });
            var vm = new DefinitionsViewModel
            {
                BrandsView = new _DefinitionsBrandsViewModel
                {
                    Brands = dbContext
                    .Brands
                    .Select(b => new _DefinitionsBrandsViewModel.Brand
                    {
                        BrandId = b.BrandId,
                        BrandName = b.BrandName
                    })
                    .ToList(),
                    SelectedBrandId = 1
                },
                ModelsView=new _DefinitionsModelsViewModel{
                    Models=dbContext
                    .Models
                    .Select(m=>new _DefinitionsModelsViewModel.Model{
                        ModelId=m.ModelId,
                        ModelName=m.ModelName
                    })
                    .ToList()
                }
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult _tanimlar_markalar([FromBody] int? SelectedId){
            var vm=new _DefinitionsBrandsViewModel{
                Brands = dbContext
                    .Brands
                    .Select(b => new _DefinitionsBrandsViewModel.Brand
                    {
                        BrandId = b.BrandId,
                        BrandName = b.BrandName
                    })
                    .ToList(),
                    SelectedBrandId=SelectedId??1
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult _tanimlar_modeller([FromBody] int? BrandId){
            var vm=new _DefinitionsModelsViewModel{
                Models=dbContext
                .Models
                .Where(m=>m.BrandId==BrandId||BrandId==null)
                .Select(m=>new _DefinitionsModelsViewModel.Model{
                    ModelId=m.ModelId,
                    ModelName=m.ModelName
                }).ToList()
            };
            return View(vm);
        }


        #endregion
    }
}