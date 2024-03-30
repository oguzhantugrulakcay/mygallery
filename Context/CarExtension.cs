using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class CarExtension
{
    public int ExtensionId { get; set; }

    public string ExtensionName { get; set; } = null!;

    public virtual ICollection<BuyRequestExtension> BuyRequestExtensions { get; set; } = new List<BuyRequestExtension>();
}
