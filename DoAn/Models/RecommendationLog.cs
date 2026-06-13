using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class RecommendationLog
{
    public long Id { get; set; }

    public long InputProductId { get; set; }

    public long RecommendedProductId { get; set; }

    public long? RuleId { get; set; }

    public decimal? ConfidenceValue { get; set; }

    public decimal? LiftValue { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product InputProduct { get; set; } = null!;

    public virtual Product RecommendedProduct { get; set; } = null!;

    public virtual AssociationRule? Rule { get; set; }
}
