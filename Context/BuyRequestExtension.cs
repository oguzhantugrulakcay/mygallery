using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class BuyRequestExtension
{
    public int RequestExtensionId { get; set; }

    public int RequestId { get; set; }

    public int ExtensionId { get; set; }

    public bool IsHave { get; set; }

    public virtual CarExtension Extension { get; set; }

    public virtual BuyRequest Request { get; set; }
}
