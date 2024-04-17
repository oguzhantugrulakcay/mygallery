namespace mygallery.Models.ViewModels
{
    public class _DefinitionsBrandsViewModel
    {
        public _DefinitionsBrandsViewModel(){
            Brands=new List<Brand>();
            SelectedBrandId=1;
        }
        public class Brand{
            public int BrandId { get; set; }
            public string BrandName { get; set; }
        }
        public List<Brand> Brands {get;set;}
        public int SelectedBrandId { get; set; }
    }
}