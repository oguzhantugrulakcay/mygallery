using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mygallery.Data;

namespace mygallery.Models
{
    public class RequestSearchData:Pageable
    {
        public int? ModelId { get; set; }
        public int? BrandId { get; set; }
        public int? Year { get; set; }
        public string Requester { get; set; }
        public bool? Status { get; set; }
        
    }
}