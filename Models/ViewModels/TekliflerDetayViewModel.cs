using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace mygallery.Models.ViewModels
{
    public class TekliflerDetayViewModel
    {
        public int RequestId { get; set; }
        public string BrandName { get; set; }
           public string ModelName { get; set; }
    public int Year { get; set; }
    public string GearType { get; set; }
    public string FuelType { get; set; }
    public List<CarPartInfo> CarInfo { get; set; }
    public List<string> Extensions { get; set; }
    public string Infos { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNo { get; set; }
    public long Km { get; set; }
    public string CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public decimal? Price { get; set; }
    public class CarPartInfo
{
    public string Part { get; set; }
    public string Status { get; set; }
}
    }
}