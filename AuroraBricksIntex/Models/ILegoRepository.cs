namespace AuroraBricksIntex.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Order> Orders { get; }

        public IQueryable<SimilarProductAnalysis> SimilarProducts { get; }

        // Crud functionality
        public void AddProduct(Product product);

        public void EditProduct(Product product);

        public void DeleteProduct(Product product);

        void AddOrder(Order order);
        //public void AddUser(Product product);

        //public void EditUser(Product product);

        //public void DeleteUser(Product product);

    }
}
