using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Models.VIewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            IEnumerable<Product> productList = _unityOfWork.Product.GetAll(includePropeties: "Category");
            return View(productList);
        }

        public IActionResult ProductDetail(int? Id)
        {
            if (Id != null)
            {
                Product individualProduct = _unityOfWork.Product.Get(u => u.Id == Id, includePropeties: "Category");

                ProductViewModel product = new()
                {
                    product = individualProduct,
                    CategoryList = null
                };

                return View(product);
            }
            return NotFound();
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
