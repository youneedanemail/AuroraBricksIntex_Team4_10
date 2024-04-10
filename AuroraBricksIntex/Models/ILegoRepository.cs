namespace AuroraBricksIntex.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }

       // void SaveChanges();   // needed for crud functionality
    }
}
