using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlulkyBook.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        public CartController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }
        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingList = new()
            {
                ShoppingCartList = _unityOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includePropeties : "Product"),
            };

            foreach(var item in shoppingList.ShoppingCartList)
            {
                item.Price = ProductPrice(item);
                shoppingList.OrderTotal += (item.Product.Price * item.Count);
            }

            return View(shoppingList);
        }

        private double ProductPrice(ShoppingCart ShoppingCart)
        {
            if (ShoppingCart.Count < 50)
            {
                return ShoppingCart.Product.Price;
            }
            else
            {
                if (ShoppingCart.Count < 100)
                {
                    return ShoppingCart.Product.Price50;
                }
                else
                {
                    return ShoppingCart.Product.Price100;
                }
            }
        }
    }
}
