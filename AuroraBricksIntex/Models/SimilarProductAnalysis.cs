using System;
using System.Collections.Generic;
namespace AuroraBricksIntex.Models;
public partial class SimilarProductAnalysis
{
    public int SimilarProductId { get; set; }
    public int ProductId { get; set; }
    public int? Recommendation1 { get; set; }
    public int? Recommendation2 { get; set; }
    public int? Recommendation3 { get; set; }
}