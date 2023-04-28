using ecommerceapp.Data;
using ecommerceapp.Models;
using ecommerceapp.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace ecommerceapp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iwebhost;

        
        private readonly ProductViewModel _viewModel;

        public ProductController(ApplicationDbContext dbContext, IWebHostEnvironment iwebhost)
        {
            _viewModel = new ProductViewModel(dbContext);
            _iwebhost = iwebhost;
        }

        public IActionResult Index()
        {
            var products = _viewModel.GetProducts();
            return View(products.ToList());

        }
        public IActionResult Create(int id)
        {
            if (id > 0)
            {
                var model = new Product();

                //model = _db.product.Find(id);
                //model.categories = _db.category.ToList();
                model = _viewModel.GetProductById(id);
               
                model.categories = _viewModel.GetAllCategories();

                return View(model);
            }
            else
            {
                var model = new Product();
                //  model.categories = _db.category.ToList();
                model.categories = _viewModel.GetAllCategories();

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product obj, IFormFile productimg)
        {
       
            if (obj.productName != null)
            {
                if (productimg != null)
                {
                    string wwwroot = _iwebhost.WebRootPath;
                    string imgtext = Path.GetExtension(productimg.FileName);
                    if (imgtext == ".jpg" || imgtext == ".png")
                    {
                        var saveimg = Path.Combine(wwwroot, "img", productimg.FileName);

                        //bool imgexist = _db.product.Where(x => x.prodimg == productimg.FileName).Any();
                        bool imgexist = _viewModel.IsImageExist(productimg);

                        if (imgexist)
                        {
                            {
                                TempData["errorimg"] = "*** Image Is Already Exist Add New Image";
                                return RedirectToAction("Index");
                            }
                        }
       
                        using (var stream = new FileStream(saveimg, FileMode.Create))
                        {
                            await productimg.CopyToAsync(stream);
                        }

                        if (obj.prodimg != null)
                        {
                            var oldpath = Path.Combine(wwwroot, "img", obj.prodimg);

                            if (System.IO.File.Exists(oldpath))
                            {
                                System.IO.File.Delete(oldpath);
                            }
                        }
                        await _viewModel.SaveChangesAsync();
                    }
                    obj.prodimg = productimg.FileName;
                }

                if (obj.productId == 0)
                {
                    //_db.product.Add(obj);
                    //_db.SaveChanges();
                    await _viewModel.AddProductAsync(obj);

                    TempData["sucessfull"] = "Item Added Sucessfull";
					return RedirectToAction("Index");

					
                }
                else
                {
                    //_db.product.Update(obj);
                    //_db.SaveChanges();
                    // await _viewModel.UpdateProductAsync(obj);
                    var categories = await _viewModel.GetAllCategoriesAsync();
                    await _viewModel.UpdateProductAsync(obj, categories);


                    TempData["sucessfull"] = "Item Update Sucessfull";
					return RedirectToAction("Index");

				}
			}
            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            //var getdata = _db.product.Find(id);
            var getdata = _viewModel.GetProductById(id);

            return View(getdata);
        }

        [HttpPost]
        public IActionResult Delete(Product obj)
        {
            string wwwroot = _iwebhost.WebRootPath;

            var oldpath = Path.Combine(wwwroot, "img", obj.prodimg);

            if (System.IO.File.Exists(oldpath))
            {
                System.IO.File.Delete(oldpath);
            }

            //_db.product.Remove(obj);
            //_db.SaveChanges();

            _viewModel.RemoveProduct(obj.productId);
            TempData["sucessfull"] = "Item Deleted Sucessfull";
            return RedirectToAction("Index");
        }

    }
}
