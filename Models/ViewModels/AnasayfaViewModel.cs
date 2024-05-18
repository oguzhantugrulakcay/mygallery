
namespace mygallery.Models.ViewModels
{
    public class AnasayfaViewModel
    {
        public int BuyRequestCount { get; set; }
        public int MyCarsCount { get; set; }
        public int WaitingRequestCount { get; set; }
        public class Request
        {
            public int RequestId { get; set; }
            public string BrandName { get; set; }
            public string ModelName { get; set; }
            public int Year { get; set; }
            public string FuelType { get; set; }
            public string GearType { get; set; }
        }
        public List<Request> Requests { get; set; }
    }
}