using System.Text.Json.Serialization;
using AuroraBricksIntex.Infrastructure;
namespace AuroraBricksIntex.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session; // defining a mechanism to retrieve a service object

            SessionCart cart = session?.GetJson<SessionCart>("Cart") ??
                    new SessionCart();

            cart.Session = session;

            return cart;

        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Product p, int quantity)
        {
            base.AddItem(p, quantity);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(Product p)
        {
            base.RemoveLine(p);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }
    }
}

