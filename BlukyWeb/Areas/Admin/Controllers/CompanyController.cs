using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.User_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnityOfWork _companyRepo;
        public CompanyController(IUnityOfWork companyRepo)
        {
            _companyRepo = companyRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddOrUpdate(int? Id)
        {
            Company companyModel;
            if (Id > 0)
            {
                companyModel = _companyRepo.Company.Get(u => u.Id == Id);
                return View(companyModel);
            }
            else
            {
                companyModel = new();
            }
            return View(companyModel);
        }
        [HttpPost]
        public IActionResult AddOrUpdate(Company model)
        {
            if (model != null)
            {
                if (model.Id == 0 && ModelState.IsValid)
                {
                    _companyRepo.Company.Add(model);
                    TempData["success"] = "Product Add Succesfuly";

                }
                else
                {
                    _companyRepo.Company.Update(model);
                    TempData["success"] = "Product Update Succesfuly";
                }
                _companyRepo.Save();
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error Show";
            return View();
        }

        #region API
        public IActionResult GetAll()
        {
            IEnumerable<Company> companyList = _companyRepo.Company.GetAll().ToList();
            return Json(new { data = companyList });
        }
        public IActionResult Delete(int? Id)
        {
            if(Id != null)
            {
                Company deleteCompany = _companyRepo.Company.Get(u => u.Id == Id);
                if (deleteCompany != null)
                {
                    _companyRepo.Company.Remove(deleteCompany);
                    _companyRepo.Save();
                }
                return Json(new { message = "Company Delete Succesfuly", success = true});
            }
            return Json(new { message = "Fail Delete Proccess", success = true});
        }
        #endregion
    }
}
