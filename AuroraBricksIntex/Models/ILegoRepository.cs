namespace AuroraBricksIntex.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
    }
}
