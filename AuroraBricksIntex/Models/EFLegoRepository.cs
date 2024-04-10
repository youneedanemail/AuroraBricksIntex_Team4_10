using AuroraBricksIntex.Data;

namespace AuroraBricksIntex.Models
{
    public class EFLegoRepository : ILegoRepository
    {
        private Team410DbContext _context;
        public EFLegoRepository(Team410DbContext temp)
        {
            _context = temp;
        }
       public IQueryable<Product> Products => _context.Products;
    }
}
