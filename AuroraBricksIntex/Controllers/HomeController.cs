using AuroraBricksIntex.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using System.Diagnostics;
using AuroraBricksIntex.Models.ViewModels;
using System.Drawing.Printing;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Recommendations;
using Microsoft.AspNetCore.Authorization;
using AuroraBricksIntex.Infrastructure;

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
        public ViewResult Index(string? colorType, string? productCategoryType, int pageNum = 1, int pageSize = 10)
        {
            // pageSize = Math.Clamp(pageSize, 5, 20);
            pageSize = pageSize;

            var productData = new ProductsListViewModel
            {
                Products = _repo.Products
                .Where(x => x.Category == productCategoryType || productCategoryType == null)
                .OrderBy(x => x.Name)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize),

                Colors = _repo.Products
                .Where(x => x.PrimaryColor == colorType || colorType == null)
                .OrderBy(x => x.PrimaryColor)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = productCategoryType == null ? _repo.Products.Count() : _repo.Products.Where(x => x.Category == productCategoryType).Count()
                },

               CurrentProductType = productCategoryType,
               CurrentColorType = colorType
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
        
        public IActionResult ConfirmProductChange()
        {
            return View();
        }

        public IActionResult ConfirmProductDelete()
        {
            return View();
        }

        // Page to show details of one product
        public IActionResult ProductDetails(int ProductId)
        {
            int recommend = 4; // number of related recommendations to display
            var productDetail = new RelatedProductsViewModel
            {
                Products = _repo.Products
              .Where(x => x.ProductId == ProductId), // can use to find recommendations related to the productID passed
                                                     //.OrderBy(x => x.Name) // can filter by highest value score if wanted
                                                     //.Take(recommend),
                Recommendations = _repo.SimilarProducts
              .Where(x => x.ProductId == ProductId),
            };
            return View(productDetail);
        }


        // CRUD FUNCTIONALITY

        //PRODUCT SECTION

        // Add

        [HttpGet]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddProduct()
        {
            ViewBag.Products = _repo.Products  // populates viewbag Products
                .OrderBy(x => x.Name)
                .ToList();

            return View();
        }

        [HttpPost]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddProduct(Product response)
        {
            if (ModelState.IsValid)
            {
                _repo.AddProduct(response);
                return View("ConfirmProductChange", response);
            }
            else
            {
                // Handle invalid ModelState
                return View(response); // Return the same view with validation errors
            }
        }

        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult ProductList()
        {
            // Linq  (sequal language)
            var Products = _repo.Products
                .OrderBy(x => x.Name).ToList();

            return View(Products);
        }


        // Edit 
        [HttpGet]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult EditProduct(int id)
        {
            var recordToEdit = _repo.Products
            .Single(x => x.ProductId == id);

            ViewBag.Products = _repo.Products
                .OrderBy(x => x.Name)
                .ToList();

            return View("AddProduct", recordToEdit);
        }

        [HttpPost]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult EditProduct(Product updateInfo)
        {
            _repo.EditProduct(updateInfo);
  
            return RedirectToAction("ProductList");
        }

        // Delete 
        [HttpGet]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            var recordToDelete = _repo.Products
                .Single(x => x.ProductId == id);


            return View(recordToDelete);
        }

        // Delete 
        [HttpPost]
        [Authorize]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(Product deleteInfo)
        {
            _repo.DeleteProduct(deleteInfo);

            return RedirectToAction("ProductList");
        }

        //// USER SECTION

        //// Add

        //[HttpGet]
        //public IActionResult AddUser()
        //{
        //    ViewBag.Users = _repo.Users  // populates viewbag Users
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AddUser(User response)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        _repo.AddUser(response);         // called the same but calling AddUser from repo
        //    }

        //    return View("SuccessUserAdd", response);
        //}


        //public IActionResult UserList()
        //{
        //    // Linq  (sequal language)
        //    var Users = _repo.Users
        //        .OrderBy(x => x.Name).ToList();

        //    return View(Users);
        //}


        //// Edit 
        //[HttpGet]
        //public IActionResult EditUser(int id)
        //{
        //    var recordToEdit = _repo.Users
        //    .Single(x => x.UserId == id);

        //    ViewBag.Users = _repo.Users
        //        .OrderBy(x => x.Name)
        //        .ToList();

        //    return View("AddUser", recordToEdit);
        //}

        //[HttpPost]
        //public IActionResult EditUser(User updateInfo)
        //{
        //    _repo.EditUser(updateInfo);

        //    return RedirectToAction("UserList");
        //}

        //// Delete 
        //[HttpGet]
        //public IActionResult DeleteUser(int id)
        //{
        //    var recordToDelete = _repo.Users
        //        .Single(x => x.UserId == id);


        //    return View(recordToDelete);
        //}

        //// Delete 
        //[HttpPost]
        //public IActionResult DeleteUser(User deleteInfo)
        //{
        //    _repo.DeleteUser(deleteInfo);

        //    return RedirectToAction("UserList");
        //}


        // Order Section


        // Checkout action
        [HttpPost]
        public IActionResult Checkout(DateTime dateTime, DateOnly dateOnly, TimeOnly timeOnly)
        {
            // Retrieve cart from session
            var cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            // Create a new order
            var order = new Order
            {
                Date = dateOnly,
                Time = timeOnly,
                DayOfWeek = dateTime.DayOfWeek.ToString(),
                EntryMode = "CVC",
                TypeOfTransaction = "Online",
                CountryOfTransaction = "USA",
                ShippingAddress = "USA",
                Bank = "Barclays",
                TypeOfCard = "Visa",
            };

            // Add line items to the order
            foreach (var line in cart.Lines)
            {
                order.LineItems.Add(new LineItem
                {
                    ProductId = line.Product.ProductId,
                    Qty = (byte)line.Quantity,


                    // You may need to adjust other properties based on your requirements
                });
            }

            // Save the order to the database
            _repo.AddOrder(order);

            // Clear the cart after checkout
            cart.Clear();
            HttpContext.Session.SetJson("cart", cart);

            // Redirect to a confirmation page
            return RedirectToAction("ConfirmOrderAdd");
        }

        public IActionResult ConfirmOrderAdd()
        {
            // Retrieve the most recently added order based on timestamp
            Order order = _repo.Orders.OrderByDescending(o => o.TransactionId).FirstOrDefault();

            // Check if the order is not null
            if (order != null)
            {
                return View(order);
            }
            else
            {
                // If no order is found, handle the situation accordingly, such as redirecting to an error page
                return RedirectToAction("Error", "Home"); // Example redirection to the error page
            }
        }


        // Faudulant Order View
        public IActionResult OrderList()
        {
            int numFraud = 25;

            var Orders = _repo.Orders
                .Where(x => x.Fraud == true)
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Time)
                .Take(numFraud).ToList();

            return View(Orders);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
