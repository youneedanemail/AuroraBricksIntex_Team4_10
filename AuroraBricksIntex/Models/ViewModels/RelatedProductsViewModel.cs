namespace AuroraBricksIntex.Models.ViewModels
{
    public class RelatedProductsViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public string? CurrentSelectedProduct { get; set; }

        public string? CurrentRelatedtedProduct { get; set; }
    }
}
