using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlulkyBook.Web.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IUnityOfWork _unityOfWork;

        public CartViewComponent(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var clameUserName = (ClaimsIdentity)User.Identity;
            //var userId = clameUserName.FindFirst(ClaimTypes.NameIdentifier);

            var userId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, _unityOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId).Count());
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
