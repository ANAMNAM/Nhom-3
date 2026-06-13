using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class MiningTransaction
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public string TransactionCode { get; set; } = null!;

    public int? TotalItems { get; set; }

    public DateTime? TransactionDate { get; set; }

    public bool? IsUsedForTraining { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<MiningTransactionItem> MiningTransactionItems { get; set; } = new List<MiningTransactionItem>();

    public virtual Order Order { get; set; } = null!;
}
