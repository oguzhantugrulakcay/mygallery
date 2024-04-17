using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mygallery.Models.ViewModels
{
    public class SatinAlmaViewModel
    {
        public class Extension{
            public int ExtensionId { get; set; }
            public string ExtensionName { get; set; }
        }
        public List<Extension> Extensions { get; set; }
    }
}