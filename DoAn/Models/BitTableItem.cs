using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class BitTableItem
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string BitVector { get; set; } = null!;

    public int SupportCount { get; set; }

    public decimal? SupportValue { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
