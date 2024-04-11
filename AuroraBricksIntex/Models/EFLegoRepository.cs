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
        public IQueryable<Order> Orders => _context.Orders;

        public void AddProduct(Product product)
        {
            _context.Add(product);
            _context.SaveChanges();
        }

        public void AddOrder(Order order)
        {
            if (order.LineItems == null)
            {
                order.LineItems = new List<LineItem>();
            }

            order.Amount = (short?)order.LineItems.Sum(item => {
                if (item != null && item.Product != null)
                    return item.Qty * item.Product.Price;
                else
                    return 0; // Or handle this case as needed, maybe throw an exception or log an error
            });

            _context.Orders.Add(order);
            _context.SaveChanges();
        }



        //public void AddUser(User user)
        //{
        //    _context.Add(user);
        //    _context.SaveChanges();
        //}

        public void DeleteProduct(Product product)
        {
            _context.Remove(product);
            _context.SaveChanges();
        }

        //public void DeleteUser(User user)
        //{
        //    _context.Remove(user);
        //    _context.SaveChanges();
        //}

        public void EditProduct(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
        }


        //public void EditUser(User user)
        //{
        //    _context.Update(user);
        //    _context.SaveChanges();
        //}
    }
}
