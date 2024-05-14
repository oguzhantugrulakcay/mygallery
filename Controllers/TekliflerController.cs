using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace mygallery.Controllers
{
    public class TekliflerController : BaseController
    {
        [HttpGet]
        public IActionResult Liste()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Detay(int id){
            return View();
        }
    }
}