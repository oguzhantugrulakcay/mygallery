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

            var vm=new AnasayfaViewModel{
                BuyRequestCount=dbContext.BuyRequests.Where(r=>r.CreatedAt.Date==DateTime.Today).Count(),
                MyCarsCount=0,
                WaitingRequestCount=dbContext.BuyRequests.Where(r=>!r.IsCompleted).Count(),
                Requests=dbContext.BuyRequests.Where(r=>!r.IsCompleted)
                .Select(r=>new AnasayfaViewModel.Request{
                    RequestId=r.RequestId,
                    BrandName=r.Model.Brand.BrandName,
                    ModelName=r.Model.ModelName,
                    FuelType=r.FuelType,
                    GearType=r.GearType,
                    Year=r.Year
                }).ToList()
            };
            return View(vm);
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
                },
                ExtensionsView=new _DefinitionsExtensionsViewModel{
                    Extensions=dbContext
                    .CarExtensions
                    .Select(e=>new _DefinitionsExtensionsViewModel.Extension{
                        ExtensionId=e.ExtensionId,
                        ExtensionName=e.ExtensionName
                    }).ToList()
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
            return View("_tanimlar_markalar",vm);
        }

        [HttpPost]
        public JsonResult json_upsert_brand([FromBody] NullableIntStringParam data){
            
            if(string.IsNullOrWhiteSpace(data.Text))
                return Json(new Result(false,"Lütfen marka adı giriniz!"));

            var hasName=dbContext
            .Brands
            .Where(b=>b.BrandName.ToLower()==data.Text.ToLower())
            .Any();

            if(hasName)
                return Json(new Result(false,"Marka adı hali hazırda kayıtlı!"));
            
            if(data.Id.HasValue){
                var brand=dbContext
                .Brands
                .FirstOrDefault(b=>b.BrandId==data.Id);


                brand.BrandName=data.Text;
            }else{
                var justBrand=new Brand{
                    BrandName=data.Text
                };
                dbContext.Brands.Add(justBrand);
            }
            dbContext.SaveChanges();
            return Json(new Result(true,"İşlem gerçekleştirildi"));
        }

        [HttpPost]
        public JsonResult json_delete_brand([FromBody] IntParam data){
            var brand=dbContext
            .Brands
            .FirstOrDefault(b=>b.BrandId==data.Id);

            if(brand==null)
                return Json(new Result(false,"Marka bulunamadı, lütfen kontrol ediniz."));

            var models=brand.Models.Select(m=>m.ModelId).ToList();

            var hasRequest=dbContext
            .BuyRequests
            .Where(c=>models.Contains(c.ModelId))
            .Any();

            if(hasRequest)
                return Json(new Result(false,"Bu markaya ait talepler olduğundan silinemez."));

            dbContext.Models.RemoveRange(brand.Models);
            dbContext.Brands.Remove(brand);
            dbContext.SaveChanges();
            return Json(new Result(true,"Marka silindi."));
        }

        [HttpPost]
        public IActionResult _tanimlar_modeller([FromBody] IntParam data){
            var vm=new _DefinitionsModelsViewModel{
                Models=dbContext
                .Models
                .Where(m=>m.BrandId==data.Id||data.Id==0)
                .Select(m=>new _DefinitionsModelsViewModel.Model{
                    ModelId=m.ModelId,
                    ModelName=m.ModelName
                }).ToList()
            };
            return View("_tanimlar_modeller",vm);
        }

        [HttpPost]
        public JsonResult json_upsert_model([FromBody] ModelUpsertData data){
            if(string.IsNullOrWhiteSpace(data.ModelName)){
                return Json(new Result(false,"Model adı boş bırakılamaz lütfen kontrol ediniz."));
            }

            if(data.BrandId==0){
                return Json(new Result(false,"Lütfen marka seçiniz!"));
            }

            var hasModalName=dbContext
                .Models
                .Any(m=>m.ModelName==data.ModelName && m.BrandId==data.BrandId);

                if(hasModalName){
                    return Json(new Result(false,"Model zaten mevcut, lütfen kontrol ediniz."));
                }

            if(!data.ModelId.HasValue){
                var hasBrand=dbContext
                .Brands
                .Any(m=>m.BrandId==data.BrandId);
                if(!hasBrand){
                    return Json(new Result(false,"Marka bulunamadı, lütfen kontrol ediniz."));
                }
                

                var justModel=new Model{
                    BrandId=data.BrandId,
                    ModelName=data.ModelName
                };
                dbContext.Models.Add(justModel);
                dbContext.SaveChanges();
                return Json(new Result(true,"Model eklendi."));
            }else{
                var model=dbContext
                    .Models
                    .FirstOrDefault(m=>m.ModelId==data.ModelId);
                
                if(model==null){
                    return Json(new Result(false,"Model bulunamadı, lütfen kontrol ediniz."));
                }

                model.ModelName=data.ModelName;
                dbContext.SaveChanges();
                return Json(new Result(true,"Model adı güncellendi"));
            }
        }

        [HttpPost]
        public JsonResult json_delete_model([FromBody] IntParam data){
            var model=dbContext
            .Models
            .FirstOrDefault(m=>m.ModelId == data.Id);

            if(model==null){
                return Json(new Result(false,"Model bulunamadı lütfen kontrol ediniz."));
            }
            if(model.BuyRequests.Count()>0){
                return Json(new Result(false,"Bu modele ait talep bulunduğundan silinemez!"));
            }

            dbContext.Models.Remove(model);
            dbContext.SaveChanges();

            return Json(new Result(true,"Model silindi."));
        }

        [HttpPost]
        public IActionResult _tanimlar_ozellikler(){
            var vm=new _DefinitionsExtensionsViewModel{
                Extensions=dbContext
                .CarExtensions
                .Select(e=>new _DefinitionsExtensionsViewModel.Extension{
                    ExtensionId=e.ExtensionId,
                    ExtensionName=e.ExtensionName
                }).ToList()
            };
            return View("_tanimlar_ozellikler",vm);
        }

        [HttpPost]
        public JsonResult json_upsert_extension([FromBody] NullableIntStringParam data){
            if(string.IsNullOrWhiteSpace(data.Text)){
                    return Json(new Result(false,"Lütfen özellik giriniz."));
            }

            var hasExtension=dbContext
            .CarExtensions
            .Any(e=>e.ExtensionName.ToLower()==data.Text.ToLower());

            if(hasExtension){
                return Json(new Result(false,"Bu özellik zaten mevcut, lütfen kontrol ediniz."));
            }

            if(data.Id.HasValue){
                var extension=dbContext
                .CarExtensions
                .FirstOrDefault(e=>e.ExtensionId==data.Id);
                
                if(extension==null){
                    return Json(new Result(false,"Özellik bulunamadı, lütfen kontrol ediniz."));
                }   

                extension.ExtensionName=data.Text;
                dbContext.SaveChanges();
                return Json(new Result(true,"Özellik güncellendi."));
            }else{
                var justExtension=new CarExtension{
                    ExtensionName=data.Text,
                };
                dbContext.CarExtensions.Add(justExtension);
                dbContext.SaveChanges();
                return Json(new Result(true,"Özellik eklendi"));
            }
        }


        [HttpPost]
        public JsonResult json_delete_extension([FromBody] IntParam data){
            var extension=dbContext
            .CarExtensions
            .FirstOrDefault(e=>e.ExtensionId==data.Id);

            if(extension==null)
            {
                return Json(new Result(false,"Özellik bulunamadı lütfen kontrol ediniz."));
            }
            dbContext.BuyRequestExtensions.RemoveRange(extension.BuyRequestExtensions);
            dbContext.CarExtensions.Remove(extension);
            dbContext.SaveChanges();
            return Json(new Result(true,"Özellik silindi"));
        }     


        #endregion
    }
}