using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
