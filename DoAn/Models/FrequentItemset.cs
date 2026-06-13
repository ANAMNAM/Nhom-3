using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class FrequentItemset
{
    public long Id { get; set; }

    public string ItemsetCode { get; set; } = null!;

    public int ItemsetSize { get; set; }

    public int SupportCount { get; set; }

    public decimal SupportValue { get; set; }

    public decimal MinSupport { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual ICollection<FrequentItemsetItem> FrequentItemsetItems { get; set; } = new List<FrequentItemsetItem>();
}
