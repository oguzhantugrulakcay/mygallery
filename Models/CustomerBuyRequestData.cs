public class CustomerBuyRequestData
{
    public CustomerBuyRequestData(){
        CarInfo=new List<CarPartInfo>();
    }
    public int ModelId { get; set; }
    public int Year { get; set; }
    public string GearType { get; set; }
    public string FuelType { get; set; }
    public List<CarPartInfo> CarInfo { get; set; }
    public List<int> ExtensionIds { get; set; }
    public string Infos { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNo { get; set; }
    public long Km { get; set; }
    public class CarPartInfo
{
    public string Part { get; set; }
    public string Status { get; set; }
}
}