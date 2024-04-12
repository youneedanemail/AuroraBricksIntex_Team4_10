namespace AuroraBricksIntex.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public virtual void AddItem(Product proj, int quantity)
        {
            CartLine line = Lines
                .Where(x => x.Product.ProductId == proj.ProductId)
                .FirstOrDefault();

            // Has this item already been added to our cart?
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = proj,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Product proj) => Lines.RemoveAll(x => x.Product.ProductId == proj.ProductId);
        public virtual void Clear() => Lines.Clear();
        public decimal CalculateTotal() => Lines.Sum(x => x.Product.Price * x.Quantity);

        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
