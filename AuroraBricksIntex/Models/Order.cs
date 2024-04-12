using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuroraBricksIntex.Models;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("transaction_ID")]
    public int TransactionId { get; set; }

    public int CustomerId { get; set; }

    public DateOnly Date { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public TimeOnly Time { get; set; }

    public string EntryMode { get; set; } = null!;

    public short? Amount { get; set; }

    public string TypeOfTransaction { get; set; } = null!;

    public string CountryOfTransaction { get; set; } = null!;

    public string? ShippingAddress { get; set; }

    public string Bank { get; set; } = null!;

    public string TypeOfCard { get; set; } = null!;

    public bool Fraud { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();
}
