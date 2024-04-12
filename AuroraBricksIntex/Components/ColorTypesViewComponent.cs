using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuroraBricksIntex.Components
{
    public class ColorTypesViewComponent : ViewComponent
    {
        private ILegoRepository _repo;
        //constructor
        public ColorTypesViewComponent(ILegoRepository temp)
        {
            _repo = temp;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedColorType = RouteData?.Values["colorType"];
            var colorTypes = _repo.Products
               .Select(x => x.PrimaryColor)
               .Distinct()
               .OrderBy(x => x);

            return View(colorTypes);
        }

    }
}
