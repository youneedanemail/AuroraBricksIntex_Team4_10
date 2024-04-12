using AuroraBricksIntex.Infrastructure;
using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Evaluation;
using Microsoft.Identity.Client;

namespace AuroraBricksIntex.Pages
{
    public class CartModel : PageModel
    {
        private ILegoRepository _repo;
        public Cart Cart { get; set; }
        public CartModel(ILegoRepository temp, Cart cartService)
        {
            _repo = temp;
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

        //remove something from the cart

        public IActionResult OnPostRemove(int productId, string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl => cl.Product.ProductId == productId).Product);
            return RedirectToPage(new { returnUrl = returnUrl });
        }


    }
}
