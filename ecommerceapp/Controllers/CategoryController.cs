using ecommerceapp.Data;
using ecommerceapp.Models;
using ecommerceapp.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace ecommerceapp.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IWebHostEnvironment _iwebhost;

        private readonly CategoryViewModel _viewModel;

        public CategoryController(ApplicationDbContext dbContext, IWebHostEnvironment iwebhost)
        {
            _viewModel = new CategoryViewModel(dbContext);
            _iwebhost = iwebhost;
        }

        public IActionResult Index()
		{
            //IEnumerable<Categories> objCategoryList = _db.category;

            var objCategoryList = _viewModel.GetAllCategories();

            return View(objCategoryList);

        }

        public IActionResult Create(int id)
        {
            if (id > 0)
            {
                var model = new Categories();
                //model = _db.category.Find(id);
                 model = _viewModel.GetCategoryById(id);

                return View(model);
            }
            else {
				var model = new Categories();
				return View(model);
			}
        }
        [HttpPost]
        public async Task<IActionResult> Create(Categories obj, IFormFile categoryimg)
        {
            
            if (obj.CategoryName != null)
            {
                if (categoryimg != null)
                {
                    string wwwroot = _iwebhost.WebRootPath;
                    string imgtext = Path.GetExtension(categoryimg.FileName);
                    if (imgtext == ".jpg" || imgtext == ".png")
                    {
                        var saveimg = Path.Combine(wwwroot, "img", categoryimg.FileName);

                        //bool imgexist = _db.category.Where(x => x.img == categoryimg.FileName).Any();
                        bool imgexist = _viewModel.IsImageExist(categoryimg);

                        if (imgexist)
                        {
                            {
                                TempData["errorimg"] = "*** Image Is Already Exist Add New Image";
                                return RedirectToAction("Index");
                            }
                        }
                        
                        using (var stream = new FileStream(saveimg, FileMode.Create))
                        {
                            await categoryimg.CopyToAsync(stream);
                        }

                        if (obj.img != null)
                        {
                            var oldpath = Path.Combine(wwwroot, "img", obj.img);

                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }
                        //await _db.SaveChangesAsync();
                       await _viewModel.SaveChangesAsync();

                    }
                    obj.img = categoryimg.FileName;
                }
                
                if (obj.CategoryId == 0)
                {
                    //_db.category.Add(obj);
                    //_db.SaveChanges();
                    await _viewModel.AddCategoryAsync(obj);

                    TempData["sucessfull"] = "**Category Added Sucessfull";
					return RedirectToAction("Index");

                }
                else
                {
                    //_db.category.Update(obj);
                    //_db.SaveChanges();
                    await _viewModel.UpdateCategoryAsync(obj);

                    TempData["sucessfull"] = "**Category Updated Sucessfull";
					return RedirectToAction("Index");
				}
			}
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            //var getdata = _db.category.Find(id);
            var getdata = _viewModel.GetCategoryById(id);

            return View(getdata);
        }

        [HttpPost]
        public IActionResult Delete(Categories obj)
        {
            string wwwroot = _iwebhost.WebRootPath;

            var oldpath = Path.Combine(wwwroot, "img", obj.img);

            if (System.IO.File.Exists(oldpath))
            {
                System.IO.File.Delete(oldpath);
            }

            //_db.category.Remove(obj);
            //_db.SaveChanges();

            _viewModel.RemoveProduct(obj.CategoryId);
            TempData["sucessfull"] = "**Category Deleted Sucessfull";
            return RedirectToAction("Index");
        }

    }
}
