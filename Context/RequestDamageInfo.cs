using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class RequestDamageInfo
{
    public int InfoId { get; set; }

    public int RequestId { get; set; }

    public bool RightFront { get; set; }

    public bool RightFrontDoor { get; set; }

    public bool RightBackDoor { get; set; }

    public bool RightBack { get; set; }

    public bool Front { get; set; }

    public bool FrontTop { get; set; }

    public bool Top1 { get; set; }

    public bool BackTop { get; set; }

    public bool Back { get; set; }

    public bool LeftFront { get; set; }

    public bool LeftFrontDoor { get; set; }

    public bool LeftBackDoor { get; set; }

    public bool LeftBack { get; set; }

    public virtual BuyRequest Request { get; set; } = null!;
}
