using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using System.Diagnostics;
using AuroraBricksIntex.Models.ViewModels;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;

namespace AuroraBricksIntex.Controllers
{
    public class HomeController : Controller
    {
        private ILegoRepository _repo;

        public HomeController(ILegoRepository temp)
        {
            _repo = temp;
        }

        //pagination info and filtering to categories
        public ViewResult Index(string? productCategoryType, int pageNum = 1)
        {
           int pageSize = 10;

            var productData = new ProductsListViewModel
            {
                Products = _repo.Products
                .Where(x => x.Category == productCategoryType || productCategoryType == null)
                .OrderBy(x => x.Name)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = productCategoryType == null ? _repo.Products.Count() : _repo.Products.Where(x => x.Category == productCategoryType).Count()
                },

               CurrentProductType = productCategoryType
            };

            return View(productData);
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
        public IActionResult ProductDetails(int ProductId)
        {
            int recommend = 4;  // number of related recommendations to display

            var productDetail = new RelatedProductsViewModel
            {
                Products = _repo.Products
                .Where(x => x.ProductId == ProductId)   // can use to find recommendations related to the productID passed
                //.OrderBy(x => x.Name)  // can filter by highest value score if wanted
                .Take(recommend),
            };

            return View(productDetail);
        }


        //// CRUD FUNCTIONALITY

        //// Add

        //[HttpGet]
        //public IActionResult AddProduct()
        //{
        //    ViewBag.Products = _repo.Products  // populates viewbag Products
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AddProduct(Product response)
        //{
        //    _repo.Products.Add(response);    // Add record to the database
        //    _repo.SaveChanges();             // commits changes to database

        //    return View("SuccessProductAdd", response);
        //}


        //public IActionResult ProductList()
        //{
        //    // Linq  (sequal language)
        //    var Products = _repo.Products
        //        .OrderBy(x => x.Name).ToList();

        //    return View(Products);
        //}


        //// Edit 
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var recordToEdit = _repo.Products
        //    .Single(x => x.ProductId == id);

        //    ViewBag.Products = _repo.Products  
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    return View("AddProduct", recordToEdit);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product updateInfo)
        //{
        //    _repo.Update(updateInfo);
        //    _repo.SaveChanges();


        //    return RedirectToAction("ProductList");
        //}

        //// Delete 
        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    var recordToDelete = _repo.Products
        //        .Single(x => x.ProductId == id);


        //    return View(recordToDelete);
        //}

        //// Delete 
        //[HttpPost]
        //public IActionResult Delete(Product deleteInfo)
        //{
        //    _repo.Products.Remove(deleteInfo);
        //    _repo.SaveChanges();

        //    return RedirectToAction("ProductList");
        //}
    


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
