using BlulkyBook.DataAccess;
using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.User_Admin)]
    public class CategoryController : Controller
    {
        public readonly IUnityOfWork _unityOfWork;
        public CategoryController(IUnityOfWork unityofwork)
        {
            _unityOfWork = unityofwork;
        }
        public IActionResult Index()
        {
            List<Category> categories = _unityOfWork.Category.GetAll().ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Test will not valid Name");
            }
            if (ModelState.IsValid)
            {
                _unityOfWork.Category.Add(obj);
                _unityOfWork.Save();
                TempData["success"] = "Category Created Succesfuly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error Show";
            return View();
        }
        public IActionResult Edit(int? Id)
        {
            if (Id != null)
            {
                var category = _unityOfWork.Category.Get(u => u.Id == Id);

                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj != null)
            {
                _unityOfWork.Category.Update(obj);
                _unityOfWork.Save();
                TempData["success"] = "Category Update Succesfuly";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error Show";
            return View();
        }
        public IActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                var category = _unityOfWork.Category.Get(u => u.Id == Id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                Category deleteCategory = _unityOfWork.Category.Get(u => u.Id == id);
                if (deleteCategory != null)
                {
                    _unityOfWork.Category.Remove(deleteCategory);
                    _unityOfWork.Save();
                    TempData["success"] = "Category Delete Succesfuly";
                    return RedirectToAction("Index");
                }
            }
            TempData["error"] = "Error Show";
            return View();
        }
    }
}
