using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class BuyRequest
{
    public int RequestId { get; set; }

    public string FistName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string GsmNo { get; set; } = null!;

    public int ModelId { get; set; }

    public int Year { get; set; }

    public string GearType { get; set; } = null!;

    public string FuelType { get; set; } = null!;

    public string? ExtraExtension { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsCompleted { get; set; }=false;

    public virtual ICollection<BuyRequestExtension> BuyRequestExtensions { get; set; } = new List<BuyRequestExtension>();

    public virtual Model Model { get; set; } = null!;

    public virtual ICollection<RequestDamageInfo> RequestDamageInfos { get; set; } = new List<RequestDamageInfo>();
}
