using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class Order
{
    public long Id { get; set; }

    public string InvoiceCode { get; set; } = null!;

    public long? CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public int? TotalQuantity { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? FinalAmount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<MiningTransaction> MiningTransactions { get; set; } = new List<MiningTransaction>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
