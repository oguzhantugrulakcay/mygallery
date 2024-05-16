using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class RequestDamageInfo
{
    public int InfoId { get; set; }

    public int RequestId { get; set; }

    public string PartName { get; set; } = null!;

    public string Damage { get; set; } = null!;

    public virtual BuyRequest Request { get; set; } = null!;
}
