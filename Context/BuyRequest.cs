using System;
using System.Collections.Generic;

namespace mygallery.Context;

public partial class BuyRequest
{
    public int RequestId { get; set; }

    public string FistName { get; set; }

    public string LastName { get; set; }

    public string GsmNo { get; set; }

    public int ModelId { get; set; }

    public int Year { get; set; }

    public string GearType { get; set; }

    public string FuelType { get; set; }

    public string ExtraExtension { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsCompleted { get; set; }

    public long Km { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<BuyRequestExtension> BuyRequestExtensions { get; set; } = new List<BuyRequestExtension>();

    public virtual Model Model { get; set; }

    public virtual ICollection<RequestDamageInfo> RequestDamageInfos { get; set; } = new List<RequestDamageInfo>();
}
