using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mygallery.Context;
using mygallery.Data;
using mygallery.Extensions;

namespace mygallery.Controllers
{
    
    [Authorize]
    public class YonetimController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppConfig appConfig;
        public YonetimController (ILogger<HomeController> logger, MyGalleryContext context, IOptions<AppConfig> config)
        {
            dbContext = context;
            _logger = logger;
            appConfig = config.Value;
        }
        [HttpGet]
        public IActionResult Anasayfa()
        {
            var currentUser=User.GetUser<LogonUser>();
            PageInit("Anasayfa", "dashboard", "actions",
            new List<Breadcrumb>
            {
                new("Anasayfa", "/yonetim/Anasayfa")
            });
            return View();
        }
    }
}