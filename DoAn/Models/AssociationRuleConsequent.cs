using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class AssociationRuleConsequent
{
    public long Id { get; set; }

    public long RuleId { get; set; }

    public long ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual AssociationRule Rule { get; set; } = null!;
}
