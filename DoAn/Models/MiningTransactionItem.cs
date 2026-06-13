using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class MiningTransactionItem
{
    public long Id { get; set; }

    public long TransactionId { get; set; }

    public long ProductId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual MiningTransaction Transaction { get; set; } = null!;
}
