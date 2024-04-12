using AuroraBricksIntex.Data;
using Microsoft.EntityFrameworkCore;

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

        public IQueryable<SimilarProductAnalysis> SimilarProducts => _context.SimilarProductAnalyses;

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
                    return 0;
            });

            // Attach each product as an existing entity
            foreach (var lineItem in order.LineItems)
            {
                // Check if the product is already tracked by the context
                var trackedProduct = _context.Products.Find(lineItem.Product.ProductId);
                if (trackedProduct != null)
                {
                    // If tracked, use the tracked product instead of the one provided
                    lineItem.Product = trackedProduct;
                }
                else
                {
                    // If not tracked and it's a known entity, attach it
                    if (_context.Entry(lineItem.Product).State == EntityState.Detached)
                        _context.Products.Attach(lineItem.Product);
                }
            }

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


        //public void EditProduct(Product updatedProduct)
        //{
        //    var existingProduct = _context.Products.Find(updatedProduct.ProductId);

        //    if (existingProduct != null)
        //    {
        //        existingProduct.Name = updatedProduct.Name;
        //        existingProduct.Description = updatedProduct.Description;
        //        existingProduct.Price = updatedProduct.Price;
        //        existingProduct.Year = updatedProduct.Year;
        //        existingProduct.NumParts = updatedProduct.NumParts;
        //        existingProduct.ImgLink = updatedProduct.ImgLink;
        //        existingProduct.PrimaryColor = updatedProduct.PrimaryColor;
        //        existingProduct.SecondaryColor = updatedProduct.SecondaryColor;
        //        existingProduct.Category = updatedProduct.Category;
        //        _context.SaveChanges();
        //    }
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
