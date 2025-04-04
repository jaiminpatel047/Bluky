using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Utility;
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
                ShoppingCartList = _unityOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includePropeties: "Product"),
                OrderHeader = new()
            };

            foreach(var item in shoppingList.ShoppingCartList)
            {
                item.Price = ProductPrice(item);
                shoppingList.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            return View(shoppingList);
        }
        public IActionResult Plus(int cartId)
        {
            var shoppingCart = _unityOfWork.ShoppingCart.Get(u => u.Id == cartId, includePropeties: "Product");
            shoppingCart.Count += 1;
            _unityOfWork.ShoppingCart.Update(shoppingCart);
            _unityOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var shoppingCart = _unityOfWork.ShoppingCart.Get(u => u.Id == cartId, includePropeties: "Product");
                if (shoppingCart.Count > 1)
                {
                    shoppingCart.Count -= 1;
                }
                else
                {
                    _unityOfWork.ShoppingCart.Remove(shoppingCart);
                }

              _unityOfWork.ShoppingCart.Update(shoppingCart);
              _unityOfWork.Save();
          return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var shoppingCart = _unityOfWork.ShoppingCart.Get(u => u.Id == cartId, includePropeties: "Product");
            _unityOfWork.ShoppingCart.Remove(shoppingCart);
            _unityOfWork.Save();
            return RedirectToAction(nameof(Index));
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
        public IActionResult Summary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartVM shoppingList = new()
            {
                ShoppingCartList = _unityOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includePropeties: "Product"),
                OrderHeader = new()
            };

            shoppingList.OrderHeader.applicationUser = _unityOfWork.ApplicationUser.Get(x => x.Id == userId);


            if (shoppingList.OrderHeader.applicationUser != null)
            {
                shoppingList.OrderHeader.Name = shoppingList.OrderHeader.applicationUser.Name;
                shoppingList.OrderHeader.City = shoppingList.OrderHeader.applicationUser.City;
                shoppingList.OrderHeader.StreetAdress = shoppingList.OrderHeader.applicationUser.StreetAddres;
                shoppingList.OrderHeader.State = shoppingList.OrderHeader.applicationUser.State;
                shoppingList.OrderHeader.Number = shoppingList.OrderHeader.applicationUser.PhoneNumber;
                shoppingList.OrderHeader.PostalCode = shoppingList.OrderHeader.applicationUser.PostalCode;
                shoppingList.OrderHeader.ApplicationUserId = shoppingList.OrderHeader.applicationUser.Id;
            }

            foreach (var item in shoppingList.ShoppingCartList)
            {
                item.Price = ProductPrice(item);
                shoppingList.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            return View(shoppingList);
        }
        [HttpPost]
        public IActionResult Summary(ShoppingCartVM shoppingList)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = _unityOfWork.ApplicationUser.Get(x => x.Id == userId);
            shoppingList.ShoppingCartList = _unityOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includePropeties: "Product");
            shoppingList.OrderHeader.OrderDate = System.DateTime.Today;
            shoppingList.OrderHeader.ApplicationUserId = userId;

            foreach (var item in shoppingList.ShoppingCartList)
            {
                item.Price = ProductPrice(item);
                shoppingList.OrderHeader.OrderTotal += (item.Product.Price * item.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                shoppingList.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                shoppingList.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            else
            {
                shoppingList.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                shoppingList.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unityOfWork.OrderHeader.Add(shoppingList.OrderHeader);
            _unityOfWork.Save();

            foreach (var item in shoppingList.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    OrderHeaderId = shoppingList.OrderHeader.Id,
                    ProductId = item.ProductID,
                    Count = item.Count,
                    Price = item.Price
                };
                _unityOfWork.OrderDetail.Add(orderDetail);
                _unityOfWork.Save();
            }

            return RedirectToAction(nameof(OrderConfirmation), new {Id = shoppingList.OrderHeader.Id});
        }
        public IActionResult OrderConfirmation(int Id)
        {
            return View(Id);
        }
    }
}
