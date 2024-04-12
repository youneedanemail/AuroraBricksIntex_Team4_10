using AuroraBricksIntex.Infrastructure;
using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace AuroraBricksIntex.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        private UserManager<IdentityUser> _userManager;
        private Team410DbContext _context; // Assuming your DbContext that contains Customers is named Team410DbContext
        public Cart Cart { get; set; }
        
        public CartModel(ILegoRepository repo, UserManager<IdentityUser> userManager, Team410DbContext context, Cart cartService)
        {
            _repo = repo;
            _userManager = userManager;
            _context = context;
            Cart = cartService;
        }
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(int productId, string returnUrl)
        {
            Product prod = _repo.Products.FirstOrDefault(x => x.ProductId == productId);

            if(prod != null)
            {
                Cart.AddItem(prod, 1);
            }

            return RedirectToPage (new { returnUrl = returnUrl });
        }

        public async Task<IActionResult> OnPostCheckout()
        {
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("Error"); // No user logged in
            }

            var customer = _context.Customers.FirstOrDefault(c => c.AuthenticationId == user.Id); // Ensure this matches how you relate users to customers
            if (customer == null)
            {
                return RedirectToPage("Error"); // No customer record found
            }

            var order = new Order
            {
                CustomerId = customer.CustomerId, // Setting CustomerId from the found customer
                Date = DateOnly.FromDateTime(DateTime.Now), // Adjust date as needed
                Time = TimeOnly.FromDateTime(DateTime.Now), // Adjust time as needed
                DayOfWeek = DateTime.Now.DayOfWeek.ToString(),
                EntryMode = "CVC",
                TypeOfTransaction = "Online",
                CountryOfTransaction = "USA",
                ShippingAddress = "USA",
                Bank = "Barclays",
                TypeOfCard = "Visa",
                Fraud = false // You may need to implement fraud detection logic
            };

            foreach (var line in cart.Lines)
            {
                order.LineItems.Add(new LineItem
                {
                    ProductId = line.Product.ProductId,
                    Qty = (byte)line.Quantity,
                    Product = line.Product
                });
            }

            _repo.AddOrder(order);
            cart.Clear();
            HttpContext.Session.SetJson("cart", cart);
            return RedirectToAction("ConfirmOrderAdd", "Home");
        }

        //public IActionResult OnPostCheckout()
        //{
        //    var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

        //    var order = new Order
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now), // Adjust date as needed
        //        Time = TimeOnly.FromDateTime(DateTime.Now), // Adjust time as needed
        //        DayOfWeek = DateTime.Now.DayOfWeek.ToString(),
        //        EntryMode = "CVC",
        //        TypeOfTransaction = "Online",
        //        CountryOfTransaction = "USA",
        //        ShippingAddress = "USA",
        //        Bank = "Barclays",
        //        TypeOfCard = "Visa",
        //        Fraud = false // You may need to implement fraud detection logic
        //    };

        //    foreach (var line in cart.Lines)
        //    {
        //        order.LineItems.Add(new LineItem
        //        {
        //            TransactionId = order.TransactionId,
        //            ProductId = line.Product.ProductId,
        //            Qty = (byte)line.Quantity,
        //            Product = line.Product // Make sure to include this if it's not being set elsewhere
        //        });
        //    }

        //    _repo.AddOrder(order);

        //    cart.Clear();
        //    HttpContext.Session.SetJson("cart", cart);

        //    return RedirectToPage("ConfirmOrderAdd");
        //}

        //remove something from the cart

        public IActionResult OnPostRemove(int productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }


    }
}
