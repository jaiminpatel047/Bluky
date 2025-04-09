using BlulkyBook.DataAccess;
using BlulkyBook.DataAccess.Repository;
using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Models.VIewModel;
using BlulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.User_Admin)]
    public class UserManagementController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagementController(IUnityOfWork unityOfWork, UserManager<IdentityUser> userManager)
        {
            _unityOfWork = unityOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string Id)
        {
            UserManagementVM User = new UserManagementVM()
            {
                ApplicationUser = _unityOfWork.ApplicationUser.Get(x => x.Id == Id, includePropeties: "Company"),
                RoleList = _unityOfWork.ApplicationUser.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Role,
                    Value = i.Role
                })
            };
            return View(User);
        }


        #region API
        public IActionResult GetAll()
        {
           IEnumerable<ApplicationUser> userList = _unityOfWork.ApplicationUser.GetAll(includePropeties: "Company").ToList();

            foreach (var user in userList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var checkUser = _unityOfWork.ApplicationUser.Get(x => x.Id == id);

            if (checkUser == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (checkUser.LockoutEnd != null && checkUser.LockoutEnd > DateTime.Today)
            {
                checkUser.LockoutEnd = DateTime.Now;
            }
            else
            {
                checkUser.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _unityOfWork.ApplicationUser.Update(checkUser);
            _unityOfWork.Save();

            return Json(new { success = true, message = "Operation Successful" });
        }
        #endregion
    }
}
