namespace mygallery.Models.ViewModels
{
    public class _DefinitionsExtensionsViewModel
    {
        public _DefinitionsExtensionsViewModel(){
            Extensions=new List<Extension>();
        }
        public class Extension{
            public int ExtensionId { get; set; }
            public string ExtensionName { get; set; }
        }
        public List<Extension> Extensions { get; set; }
    }
}