using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuroraBricksIntex.Models;

public partial class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
