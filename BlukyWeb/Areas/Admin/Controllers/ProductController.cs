using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using BlulkyBook.Models.VIewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IO;

namespace BlulkyBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnityOfWork unityOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unityOfWork = unityOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? Id)
        {
            IEnumerable<SelectListItem> categoryList = _unityOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ProductViewModel productDetail;

            if (Id == null || Id == 0)
            {
                productDetail = new()
                {
                    product = new Product(),
                    CategoryList = categoryList
                };
            }
            else
            {
                productDetail = new()
                {
                    product = _unityOfWork.Product.Get(u => u.Id == Id),
                    CategoryList = categoryList
                };
            }

            return View(productDetail);
        }
        [HttpPost]
        public IActionResult Upsert(ProductViewModel entity, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string ProductPath = Path.Combine(wwwrootPath, @"image\product");

                    if (!string.IsNullOrEmpty(entity.product.ImageURl))
                    {
                        var OldImageName = entity.product.ImageURl.TrimStart('\\');
                        var oldImagePath = Path.Combine(wwwrootPath, OldImageName);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(ProductPath, ImageName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    entity.product.ImageURl = @"\image\product\" + ImageName;
                }
                
                if (entity.product.Id == 0)
                {
                    _unityOfWork.Product.Add(entity.product);
                    TempData["success"] = "Product Created Succesfuly";
                }
                else
                {
                    _unityOfWork.Product.Update(entity.product);
                    TempData["success"] = "Product Update Succesfuly";
                }
                _unityOfWork.Save();
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error Show";
            return View();
        }
        //public IActionResult Delete(int? ID)
        //{
        //    if (ID != null)
        //    {
        //        IEnumerable<SelectListItem> categoryList = _unityOfWork.Category.GetAll().Select(u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });

        //        ProductViewModel productDetail = new()
        //        {
        //            product = _unityOfWork.Product.Get(u => u.Id == ID),
        //            CategoryList = categoryList
        //        };
        //        return View(productDetail);
        //    }
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    if (id != null)
        //    {
        //        Product deleteProduct = _unityOfWork.Product.Get(u => u.Id == id);
        //        if (deleteProduct != null)
        //        {
        //            _unityOfWork.Product.Remove(deleteProduct);
        //            _unityOfWork.Save();
        //            TempData["success"] = "Category Delete Succesfuly";
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    TempData["error"] = "Error Show";
        //    return View();
        //}

        #region Api Call
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> productList = _unityOfWork.Product.GetAll(includePropeties: "Category");
            return Json(new { data = productList });
        }
        public IActionResult Detele(int? id)
        {
            if(id != null)
            {
                Product deleteProduct = _unityOfWork.Product.Get(u => u.Id == id);
                if (deleteProduct != null)
                {
                    var deleteProductImage = Path.Combine(_webHostEnvironment.WebRootPath, deleteProduct.ImageURl);
                    if (System.IO.File.Exists(deleteProductImage))
                    {
                        System.IO.File.Delete(deleteProductImage);
                    }
                    _unityOfWork.Product.Remove(deleteProduct);
                    _unityOfWork.Save();
                    return Json(new { message = "Category Delete Succesfuly", success = true});
                }
                return Json(new { message = "Fail Delete", success = false });
            }
            return Json(new { message = "id is null", success = false });
        }
        #endregion
    }
}
