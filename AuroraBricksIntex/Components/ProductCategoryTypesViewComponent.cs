using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using AuroraBricksIntex.Models;
using Microsoft.Build.Framework;

namespace AuroraBricksIntex.Components
{
    public class ProductCategoryTypesViewComponent : ViewComponent
    {
        private ILegoRepository _repo;
        //constructor
        public ProductCategoryTypesViewComponent(ILegoRepository temp)
        {
            _repo = temp;
        }
            public IViewComponentResult Invoke()
        {
            ViewBag.SelectedProductType = RouteData?.Values["productCategoryType"];
            var productCategoryTypes = _repo.Products
               .Select(x => x.Category)
               .Distinct()
               .OrderBy(x => x);

            return View(productCategoryTypes);
        }
    }

}
