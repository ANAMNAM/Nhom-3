using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class AssociationRule
{
    public long Id { get; set; }

    public string RuleCode { get; set; } = null!;

    public string AntecedentText { get; set; } = null!;

    public string ConsequentText { get; set; } = null!;

    public decimal SupportValue { get; set; }

    public decimal ConfidenceValue { get; set; }

    public decimal? LiftValue { get; set; }

    public decimal MinSupport { get; set; }

    public decimal MinConfidence { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public virtual ICollection<AssociationRuleAntecedent> AssociationRuleAntecedents { get; set; } = new List<AssociationRuleAntecedent>();

    public virtual ICollection<AssociationRuleConsequent> AssociationRuleConsequents { get; set; } = new List<AssociationRuleConsequent>();

    public virtual ICollection<RecommendationLog> RecommendationLogs { get; set; } = new List<RecommendationLog>();
}
