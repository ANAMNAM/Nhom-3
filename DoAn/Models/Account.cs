using System;
using System.Collections.Generic;

namespace DoAn.Models;

public partial class Account
{
    public long Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public long? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
