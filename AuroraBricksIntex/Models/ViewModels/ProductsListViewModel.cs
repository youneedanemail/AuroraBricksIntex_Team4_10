namespace AuroraBricksIntex.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductCategory { get; set; }

        public string? CurrentProductType { get; set; }


    }
}
