using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly ICategoryRepository  CategoryRpo;
        private readonly IUnitOfWork _unitofwork;
        public CategoryController(IUnitOfWork Unitwork)
        {
            _unitofwork = Unitwork;   //creating local instance of database db as _db
        }
        public IActionResult Index()
        {
            List<Category> objcategoires = _unitofwork.Category.GetAll().ToList();//calling data from table
            return View(objcategoires);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.name.ToString() == obj.DisplayOrder.ToString()|| obj.name.ToString()=="")
            {
                ModelState.AddModelError("", "Add another text here");
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Add(obj);
                TempData["sucess"] = "Record Created Successfully";
                _unitofwork.save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? dbcategory = _unitofwork.Category.Get(u=>u.id==id);
            if (dbcategory == null)
            {
                return NotFound();
            }
            return View(dbcategory);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is not a category");
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Update(obj);
                _unitofwork.save();
                TempData["sucess"] = "Record Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? dbcategory = _unitofwork.Category.Get(u => u.id == id);
            if (dbcategory == null)
            {
                return NotFound();
            }
            return View(dbcategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _unitofwork.Category.Get(u => u.id == id);
            if (obj == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitofwork.Category.Remove(obj);
                _unitofwork.save();
                TempData["sucess"] = "Record Deleted Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
