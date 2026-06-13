using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class Product
{
    public long Id { get; set; }

    public string ProductCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long CategoryId { get; set; }

    public long? BrandId { get; set; }

    public string? Unit { get; set; }

    public decimal? BasePrice { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AssociationRuleAntecedent> AssociationRuleAntecedents { get; set; } = new List<AssociationRuleAntecedent>();

    public virtual ICollection<AssociationRuleConsequent> AssociationRuleConsequents { get; set; } = new List<AssociationRuleConsequent>();

    public virtual ICollection<BitTableItem> BitTableItems { get; set; } = new List<BitTableItem>();

    public virtual Brand? Brand { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<FrequentItemsetItem> FrequentItemsetItems { get; set; } = new List<FrequentItemsetItem>();

    public virtual ICollection<MiningTransactionItem> MiningTransactionItems { get; set; } = new List<MiningTransactionItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<RecommendationLog> RecommendationLogInputProducts { get; set; } = new List<RecommendationLog>();

    public virtual ICollection<RecommendationLog> RecommendationLogRecommendedProducts { get; set; } = new List<RecommendationLog>();
}
