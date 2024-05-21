using System.Reflection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using mygallery.Context;
using mygallery.Data;
using mygallery.Extensions;
using mygallery.Models.ViewModels;

namespace mygallery.Controllers
{
    public abstract class BaseController : Controller
    {
        protected MyGalleryContext dbContext;
        protected AppConfig appConfig;
        protected IWebHostEnvironment hostingEnvironment;
        protected IDataProtector dataProtector;
        private string MenuKey = "";
        private string PageTitle = "";
        private string TabKey = "actions";
        private List<Breadcrumb> Breadcrumbs { get; set; }

        public void PageInit(string PageTitle, string MenuKey, string TabKey, List<Breadcrumb> Breadcrumbs)
        {
            this.PageTitle=PageTitle;
            this.Breadcrumbs = this.Breadcrumbs ?? new List<Breadcrumb>();
            this.Breadcrumbs.AddRange(Breadcrumbs);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
var assembly = Assembly.GetExecutingAssembly();

        var version = assembly.GetName().Version.ToString();
            if (context.Controller is Controller controller &&
                context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                 var currentUser = controller.User.GetUser<LogonUser>();
                var vm=new LayoutViewModel();
                if(currentUser!=null){
                    vm.FirstName=currentUser.FirstName;
                    vm.LastName=currentUser.LastName;
                }
                vm.Version=version;
                controller.ViewData["LayoutViewModel"] = vm;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (context.Controller is Controller controller &&
                context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                if (controller.ViewData["LayoutViewModel"] is LayoutViewModel vm)
                {
                    vm.PageTitle = PageTitle;
                    vm.MenuKey = MenuKey;
                    vm.TabKey = TabKey;
                    vm.Breadcrumbs.AddRange(Breadcrumbs ?? new List<Breadcrumb>());
                    if (vm.Breadcrumbs.Count > 0)
                        vm.Breadcrumbs.LastOrDefault().IsActive = true;
                }
        }
    }
}