using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using System.Diagnostics;

namespace AuroraBricksIntex.Controllers
{
    public class HomeController : Controller
    {
        private ILegoRepository _repo;

        public HomeController(ILegoRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index()
        {
            // this code will change with pagination
            var productData = _repo.Products;
            return View(productData);

            // Possible code for pagination

            //int pageSize = 3;

            //var productData = new ProducttsListViewModel
            //{
            //    Products = _repo.Products
            //    .Where(x => x.ProductType == productType || productType == null)
            //    .OrderBy(x => x.ProductName)
            //    .Skip(pageSize * (pageNum - 1))
            //    .Take(pageSize),

            //    PaginationInfo = new PaginationInfo
            //    {
            //        CurrentPage = pageNum,
            //        ItemsPerPage = pageSize,
            //        TotalItems = productType == null ? _repo.Products.Count() : _repo.Products.Where(x => x.ProductType == productType).Count()
            //    },
            //    CurrentProductType = productType
            //}
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Page to show details of one product
        public IActionResult ProductDetails()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
