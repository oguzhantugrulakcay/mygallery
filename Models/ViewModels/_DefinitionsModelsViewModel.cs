namespace mygallery.Models.ViewModels
{
    public class _DefinitionsModelsViewModel
    {
        public _DefinitionsModelsViewModel(){
            Models=new List<Model>();
        }
        public class Model
        {
            public int ModelId { get; set; }
            public string ModelName { get; set; }
        }
        public List<Model> Models {get;set;}
    }
}