using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class FrequentItemsetItem
{
    public long Id { get; set; }

    public long FrequentItemsetId { get; set; }

    public long ProductId { get; set; }

    public virtual FrequentItemset FrequentItemset { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
