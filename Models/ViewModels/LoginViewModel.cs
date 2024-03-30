using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mygallery.Models.ViewModels
{
    public class LoginViewModel
    {
        public string ErrorDesc { get; set; }
        public bool HasError { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public string RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}