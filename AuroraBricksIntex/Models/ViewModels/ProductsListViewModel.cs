using System.Security.Cryptography.X509Certificates;

namespace AuroraBricksIntex.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }

        public IQueryable<Product> Colors { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public string? CurrentProductCategory { get; set; }

        public string? CurrentProductType { get; set; }
        public string? CurrentColorType { get; set; }
        



    }
}
