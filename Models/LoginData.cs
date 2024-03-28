using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mygallery.Models
{
    public class LoginData
    {
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}