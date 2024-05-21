using mygallery.Data;

namespace mygallery.Models.ViewModels;

public class LayoutViewModel
{
    public LayoutViewModel()
    {
        Breadcrumbs = new List<Breadcrumb>();
        MenuKey = "";
        TabKey = "";
        Infos = new List<UserInfo>();
    }
    public string PageTitle { get; set; }
    public string Photo { get; set; } = "/users/default.png";
    public string LoggedUserName { get; set; } = "";
    public string TabKey { get; set; } = "actions";
    public string MenuKey { get; set; } = "";
    public List<Breadcrumb> Breadcrumbs { get; set; }
    public List<UserInfo> Infos { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Version { get; set; }
    public class UserInfo
    {
        public int InfoId { get; set; }
        public string InfoText { get; set; }
        public string InfoLink { get; set; }
        public DateTime InfoDate { get; set; }
    }
}