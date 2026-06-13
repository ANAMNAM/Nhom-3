using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class Category
{
    public long Id { get; set; }

    public string NameVi { get; set; } = null!;

    public string? NameEn { get; set; }

    public string? Description { get; set; }

    public long? ParentId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
