using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class Model
{
    public int ModelId { get; set; }

    public int BrandId { get; set; }

    public string ModelName { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual ICollection<BuyRequest> BuyRequests { get; set; } = new List<BuyRequest>();
}
