using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mygallery.Models
{
    public class Breadcrumb
    {
        public Breadcrumb(string Link)
    {
        this.Title = Title;
        this.Link = Link;
        IsActive = false;
        IsHome = true;
    }

    public Breadcrumb(string Title, string Link)
    {
        this.Title = Title;
        this.Link = Link;
        IsActive = IsActive;
    }

    public bool IsHome { get; set; }
    public bool IsActive { get; set; }
    public string Link { get; set; }
    public string Title { get; set; }
    }
}