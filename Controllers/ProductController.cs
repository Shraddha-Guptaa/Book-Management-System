using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork Unitwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = Unitwork;   //creating local instance of database db as _db
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //inlcude properties is used to calling data of another table by navigation property
            List<Product> objProduct = _unitofwork.Product.GetAll(includeproperties: "category").ToList();//calling data from table
            return View(objProduct);
        }
        public IActionResult Upsert(int?id)
        {
            ProductVM productvm = new()
            {
                CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.name,
                    Value = u.id.ToString()
                }),
                product = new Product()
            };
            if (id == null || id == 0)
            {

                return View(productvm);
            }
            else
            {
                productvm.product  = _unitofwork.Product.Get(u => u.ProductId == id);
              
                return View(productvm);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootpath = _WebHostEnvironment.WebRootPath;//this is to find the wwwrootfolder in any pc
                if (file != null)
                {
                    string filename=Guid.NewGuid()+Path.GetExtension(file.FileName);//sets filename in random number
                    string productpath = Path.Combine(wwwrootpath, @"Images\product");//this sets the file inside the folder i created which is inside image -- product
                    if (!string.IsNullOrEmpty(obj.product.ImageUrl))
                    {
                        //delte old image 
                        var oldimage = Path.Combine(wwwrootpath, obj.product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(oldimage))
                        {
                            System.IO.File.Delete(oldimage);
                        }

                    }
                    using (var filestream = new FileStream (Path.Combine(productpath, filename),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.product.ImageUrl = @"\Images\product\" + filename;
                }
                if ( obj.product.ProductId == 0) {
                    _unitofwork.Product.Add(obj.product);
                    TempData["sucess"] = "Record Created Successfully";

                }
                else
                {
                    _unitofwork.Product.Update(obj.product);
                    TempData["sucess"] = "Record Update Successfully";

                }
                _unitofwork.save();
                return RedirectToAction("Index");
            }
            else
            {
                obj.CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.name,
                    Value = u.id.ToString()
                });
                          return View(obj);

            }

        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Product? dbProduct = _unitofwork.Product.Get(u => u.ProductId == id);
        //    if (dbProduct == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(dbProduct);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
           
        //    if (ModelState.IsValid)
        //    {
        //        _unitofwork.Product.Update(obj);
        //        _unitofwork.save();
        //        TempData["sucess"] = "Record Updated Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Product? dbProduct = _unitofwork.Product.Get(u => u.ProductId == id);
        //    if (dbProduct == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(dbProduct);
        //}
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitofwork.Product.Get(u => u.ProductId == id);
            if (obj == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Product.Remove(obj);
                _unitofwork.save();
                TempData["sucess"] = "Record Deleted Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        #region
        //Api call for Ajax
        [HttpGet]
        public IActionResult GetData()
        {
            //inlcude properties is used to calling data of another table by navigation property
            List<Product> objProduct = _unitofwork.Product.GetAll(includeproperties: "category").ToList();//calling data from table
            return Json(new { data = objProduct });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           var dbProduct = _unitofwork.Product.Get(u => u.ProductId == id);
            if (dbProduct == null)
            {
                return Json(new {success=false,message="error while deleting"});
            }
            
                string wwwrootpath = _WebHostEnvironment.WebRootPath;//this is to find the wwwrootfolder in any pc
                                   //delte old image 
                    var oldimage = Path.Combine(wwwrootpath, dbProduct.ImageUrl.Trim('\\'));
                    if (System.IO.File.Exists(oldimage))
                    {
                        System.IO.File.Delete(oldimage);
                    }
            _unitofwork.Product.Remove(dbProduct);
            _unitofwork.save();
                return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }

}
