using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Models.VIewModel;
using BlulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BlulkyBook.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnityOfWork _unityOfWork;

        public HomeController(ILogger<HomeController> logger, IUnityOfWork unityOfWork)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unityOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId).Count());
            }
            IEnumerable<Product> productList = _unityOfWork.Product.GetAll(includePropeties: "Category");
            return View(productList);
        }
        [HttpGet]
        public IActionResult ProductDetail(int Id)
        {
            if (Id != null)
            {
                ShoppingCart shoppingCard = new() 
                { 
                    Product = _unityOfWork.Product.Get(u => u.Id == Id, includePropeties: "Category"),
                    Count = 1,
                    ProductID = Id
                };

                return View(shoppingCard);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize]
        public IActionResult ProductDetail(ShoppingCart cart)
        {
            if (cart != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var clameUserName = (ClaimsIdentity)User.Identities;
                //var userId = clameUserName.FindFirst(ClaimTypes.NameIdentifier).Value;

                cart.ApplicationUserId = userId;
                cart.Id = 0;
                var checkShoppingIDExist = _unityOfWork.ShoppingCart.Get(u => u.ProductID == cart.ProductID && u.ApplicationUserId == cart.ApplicationUserId);
                if (checkShoppingIDExist != null)
                {
                    checkShoppingIDExist.Count += cart.Count;
                    _unityOfWork.ShoppingCart.Update(checkShoppingIDExist);
                    _unityOfWork.Save();
                    HttpContext.Session.SetInt32(SD.SessionCart, _unityOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).Count());
                }
                else
                {
                    _unityOfWork.ShoppingCart.Add(cart);
                    _unityOfWork.Save();
                    HttpContext.Session.SetInt32(SD.SessionCart, _unityOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == cart.ApplicationUserId).Count());
                }
               
                TempData["success"] = "Cart updated successfully";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
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
