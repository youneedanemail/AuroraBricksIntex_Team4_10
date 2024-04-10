namespace AuroraBricksIntex.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }

        // Crud functionality
        public void AddProduct(Product product);

        public void EditProduct(Product product);

        public void DeleteProduct(Product product);
        //public void AddUser(Product product);

        //public void EditUser(Product product);

        //public void DeleteUser(Product product);

    }
}
