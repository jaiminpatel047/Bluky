using BlulkyBook.DataAccess.Repository;
using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Models.VIewModel;
using BlulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

namespace BlulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        public OrderController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int orderId)
        {
            OrderVM detail = new()
            {
                OrderHeader = _unityOfWork.OrderHeader.Get(o => o.Id == orderId, includePropeties: "applicationUser"),
                OrderDetail = _unityOfWork.OrderDetail.GetAll(o => o.OrderHeaderId == orderId, includePropeties: "Product")
            };
            return View(detail);
        }

        [HttpPost]
        [Authorize(Roles = SD.User_Admin + "," + SD.User_Employe)]
        public IActionResult UpdateOrderDetail(OrderVM model)
        {
            var orderHeaderFromDb = _unityOfWork.OrderHeader.Get(u => u.Id == model.OrderHeader.Id);
            orderHeaderFromDb.Name = model.OrderHeader.Name;
            orderHeaderFromDb.Number = model.OrderHeader.Number;
            orderHeaderFromDb.StreetAdress = model.OrderHeader.StreetAdress;
            orderHeaderFromDb.City = model.OrderHeader.City;
            orderHeaderFromDb.State = model.OrderHeader.State;
            orderHeaderFromDb.PostalCode = model.OrderHeader.PostalCode;
            if (!string.IsNullOrEmpty(model.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = model.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(model.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.Carrier = model.OrderHeader.TrackingNumber;
            }
            _unityOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unityOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";


            return RedirectToAction(nameof(Detail), new { orderId = orderHeaderFromDb.Id });
        }


        #region Api Call
        [HttpGet]
        public IActionResult GetAll(string? status)
        {
            IEnumerable<OrderHeader> detail;

            if (User.IsInRole(SD.User_Admin) || User.IsInRole(SD.User_Employe))
            {
                detail = _unityOfWork.OrderHeader.GetAll(includePropeties: "applicationUser").ToList();
            }
            else
            {
                var clamisIdentity = (ClaimsIdentity)User.Identity;
                var userId = clamisIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                detail = _unityOfWork.OrderHeader.GetAll(x => x.applicationUser.Id == userId, includePropeties: "applicationUser").ToList();
            }


            switch (status)
            {
                case "pending":
                    detail = detail.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "processing":
                    detail = detail.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "shipped":
                    detail = detail.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    detail = detail.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                case "all":
                    detail = detail.ToList();
                    break;
                default:
                    detail = detail.ToList();
                    break;

            }
            return Json(new { data = detail });
        }
        #endregion
    }
}
