namespace mygallery.Models.ViewModels
{
    public class DefinitionsViewModel
    {
        public DefinitionsViewModel(){
            BrandsView=new _DefinitionsBrandsViewModel();
            ModelsView=new _DefinitionsModelsViewModel();
        }
        public _DefinitionsBrandsViewModel BrandsView { get; set; }
        public _DefinitionsModelsViewModel ModelsView {get;set;}
    }
}