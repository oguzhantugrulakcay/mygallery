using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mygallery.Models;

namespace mygallery.Controllers
{
    public abstract class BaseController : Controller
    {
        protected AppConfig appConfig;
        protected IWebHostEnvironment hostingEnvironment;
        private string MenuKey = "";
    private string PageTitle = "";
    private List<Breadcrumb> Breadcrumbs { get; set; }

        public void PageInit(string PageTitle, string MenuKey, string TabKey, List<Breadcrumb> Breadcrumbs)
    {
        this.Breadcrumbs = this.Breadcrumbs ?? new List<Breadcrumb>();
        this.Breadcrumbs.AddRange(Breadcrumbs);
    }
    }
}