using System;
using System.Collections.Generic;

namespace AuroraBricksIntex.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string CountryOfResidence { get; set; } = null!;

    public string? Gender { get; set; }

    public string? AuthenticationId { get; set; }

    public virtual AspNetUser? Authentication { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
