using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class Model
{
    public int ModelId { get; set; }

    public int BrandId { get; set; }

    public string ModelName { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;
}
