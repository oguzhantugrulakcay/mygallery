using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mygallery.Data
{
    public class Pageable
    {
        public int skip { get; set; }

        public int take { get; set; }

        public string sortBy { get; set; }
    }
}