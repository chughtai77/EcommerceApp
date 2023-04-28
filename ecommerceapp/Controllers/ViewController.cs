using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ecommerceapp.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using System;
using Microsoft.AspNetCore.Identity;
using ecommerceapp.ViewModels.Category;
using ecommerceapp.ViewModels.View;
using ecommerceapp.ViewModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerceapp.Controllers
{
    public class ViewController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
		public ShopCartViewModel ShopCartViewModel { get; set; }
        private readonly ViewViewModel _viewModel;

        public ViewController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _viewModel = new ViewViewModel(dbContext);
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            //   IEnumerable<Categories> objCategory = _db.category;
          
            //View Bag for Category list in partial View Which is in menubar 
            IEnumerable<SelectListItem> categorylist = _viewModel.GetAllCategories().Select
            (u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            ViewBag.categorylist = categorylist;


            var objCategory = _viewModel.GetAllCategories();
            return View(objCategory);
        }
        public async Task<IActionResult> seeMoreAsync(int id)
        {
            //View Bag for Category list in partial View Which is in menubar 
            IEnumerable<SelectListItem> categorylist = _viewModel.GetAllCategories().Select
            (u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            ViewBag.categorylist = categorylist;

            // Retrieve the category from the database
            //var category = _db.category.Find(id);
            var category = await _viewModel.GetCategoryByIdAsync(id);


            // Retrieve all products that belong to the category
            //var products = _db.product.Where(p => p.cId == id).ToList();
            var products = _viewModel.GetProductsByCategoryId(id);

            // Set the category and products on a view model
            var viewModel = new IndexViewModel
            {
                Category = category,
                prodlist = (List<Product>)products
            };
            // Pass the view model to the view
            return View(viewModel);
        }

        //currently shifted to cartExtra by cartvu id in view
		public IActionResult cartvu(int productId)
		{
			Product product = _db.product.FirstOrDefault(p => p.productId == productId);

			ShopCartVM cartObj = new()
			{
				count = 1,
				ProductId = productId,
				productName = product.productName,
				productDesc = product.productDesc,
				productPrice = product.productPrice,
				prodimg = product.prodimg,

				Product = product
			};
			return View(cartObj);
		}

        [HttpPost]
        public IActionResult cartvu(ShoppingCart obj)
        {
            List<ShopCartVM> cartList = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<ShopCartVM>>("cart") ?? new List<ShopCartVM>();
            bool productExists = false;
            foreach (var item in cartList)
            {
                if (item.ProductId == obj.productId)
                {
                    item.count += obj.Quantity;
                    productExists = true;
                    break;
                }
            }
            if (!productExists)
            {
                //cartList.Add(obj);
            }
           // _db.shopCartVMs.Add(obj);
            _db.SaveChanges();
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("cart", cartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult cart()
		{
            //View Bag for Category list in partial View Which is in menubar 
            IEnumerable<SelectListItem> categorylist = _viewModel.GetAllCategories().Select
            (u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            ViewBag.categorylist = categorylist;

            string registeredUserId = HttpContext.Session.GetString("UserId");
           
                //	return RedirectToAction("Index", "Registration");
                //}
                //var cartItems = _db.shoppingCarts.Where(c => c.RegisterUserId == registeredUserId).ToList();
                //var cartItems = _db.shoppingCarts.Include(c => c.Product).Where(c => c.RegisterUserId == registeredUserId).ToList();
                var cartItems = _viewModel.GetCartItemsByUserId(registeredUserId);


            var viewModel = new ShopCartViewModel
			{
				CartItems = cartItems.ToList()
			};
			return View(viewModel);
        }

        public IActionResult guestCart()
        {
            //View Bag for Category list in partial View Which is in menubar 
            IEnumerable<SelectListItem> categorylist = _viewModel.GetAllCategories().Select
            (u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            ViewBag.categorylist = categorylist;


            List<ShoppingCart> guestCartItems = HttpContext.Session.GetObjectFromJson<List<ShoppingCart>>("cart") ?? new List<ShoppingCart>();

            // Get product details for each item in the cart
            //List<ShopCartVM> cartItems = new List<ShopCartVM>();
            List<ShopCartVM> cartItems = guestCartItems.Select((item, index) => new ShopCartVM
            {
                Id = index + 1,
                count = item.Quantity,
                Product = _viewModel.GetProductById(item.productId)
            }).ToList();


            var viewModel = new ShopCartViewModel
            {
                ListCart = cartItems.ToList()
            };

            return View(viewModel);
        }

        public IActionResult cartExtra(int productId)
        {
            //View Bag for Category list in partial View Which is in menubar 
            IEnumerable<SelectListItem> categorylist = _viewModel.GetAllCategories().Select
            (u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryId.ToString()
            });
            ViewBag.categorylist = categorylist;


            //Product product = _db.product.FirstOrDefault(p => p.productId == productId);
            Product product = _viewModel.GetProductById(productId);

            ShoppingCart cartObj = new()
            {
                Quantity = 1,
                productId = productId,
                productName = product.productName,
                productDesc = product.productDesc,
                productPrice = product.productPrice,
                prodimg = product.prodimg,
                Product = product,
                Timestamp = DateTime.UtcNow

            };
                
            var model = new ShopCartViewModel
            {
                SCart = cartObj
            };
            return View(model);
        }

        //Update 2.0
        [HttpPost]
		public async Task<IActionResult> cartExtraAsync(ShoppingCart shoppingCart)
		{

            string registeredUserId = HttpContext.Session.GetString("UserId");

            if (registeredUserId == null)
            {
                List<ShoppingCart> cartList = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<ShoppingCart>>("cart") ?? new List<ShoppingCart>();
                bool productExists = false;
                foreach (var item in cartList)
                {
                    if (item.productId == shoppingCart.productId)
                    {
                        item.Quantity += shoppingCart.Quantity;
                        productExists = true;
                        break;
                    }
                }
                if (!productExists)
                {
                    cartList.Add(shoppingCart);
                }
                // Map List<ShoppingCart> to List<ShopCartVM> and save to the database
                var shopCartVMs = cartList.Select(x => new ShopCartVM
                {
                    Id = x.Id,
                    count = x.Quantity,
                    ProductId = x.productId
                }).ToList();

                //_db.shopCartVMs.AddRange(shopCartVMs);
                //_db.SaveChanges();
                await _viewModel.AddRangeToShopCartVMsAsync(shopCartVMs);

                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("cart", cartList);
                return RedirectToAction(nameof(guestCart));
            }
            shoppingCart.RegisterUserId = registeredUserId;

            var cartObj = new ShoppingCart
            {
                Quantity = shoppingCart.Quantity,
                productId = shoppingCart.productId,
                RegisterUserId = registeredUserId,
                Timestamp = DateTime.Now // Set current timestamp

            };

            await _viewModel.AddShoppingCartToAsync(cartObj);

            return RedirectToAction("Index");
		}

		public async Task<IActionResult> plusAsync(int cartId)
		{

			string registeredUserId = HttpContext.Session.GetString("UserId");

            if (registeredUserId == null)
            {
				await _viewModel.IncrementCartItemQuantityAsync(cartId);
				return RedirectToAction("guestCart");
			}
            else {
				await _viewModel.IncrementCartItemQuantityAsync(cartId);
				return RedirectToAction("cart");
			}
			
		}

		public IActionResult minus(int cartId)
		{

			string registeredUserId = HttpContext.Session.GetString("UserId");

            if (registeredUserId == null)
            {
				_viewModel.DecreaseCartItem(cartId);
				return RedirectToAction("guestCart");
			}
            else {
				_viewModel.DecreaseCartItem(cartId);
				return RedirectToAction("cart");
			}

		}

		public async Task<IActionResult> Remove(int cartId)
		{
            
            string registeredUserId = HttpContext.Session.GetString("UserId");

            if (registeredUserId == null)
            {


                List<ShoppingCart> guestCartItems = HttpContext.Session.GetObjectFromJson<List<ShoppingCart>>("cart") ?? new List<ShoppingCart>();
                if (cartId >= 0 && cartId < guestCartItems.Count)
                {
                    guestCartItems.RemoveAt(cartId);
                    HttpContext.Session.SetObjectAsJson("cart", guestCartItems);
                }

                return RedirectToAction("guestCart");
            }
            else { 
                await _viewModel.RemoveCartItemAsync(cartId);
                return RedirectToAction("cart");
            }
            // Redirect to cart page
        }


		//CheckOut
		public IActionResult Summary()
        {
			List<ShopCartVM> cartItems = HttpContext.Session.GetObjectFromJson<List<ShopCartVM>>("cart") ?? new List<ShopCartVM>();
			var viewModel = new ShopCartViewModel { ListCart = cartItems };
			return View(viewModel);
        }

		[HttpPost]
		public IActionResult Summary(ShopCartViewModel shopCartViewModel)
		{
			List<ShopCartVM> cartItems = HttpContext.Session.GetObjectFromJson<List<ShopCartVM>>("cart") ?? new List<ShopCartVM>();
			var viewModel = new ShopCartViewModel { ListCart = cartItems };
			return View(viewModel);
		}



		public IActionResult newSummary()
		{
			List<ShopCartVM> cartItems = HttpContext.Session.GetObjectFromJson<List<ShopCartVM>>("cart") ?? new List<ShopCartVM>();
			var viewModel = new ShopCartViewModel { ListCart = cartItems };
			return View(viewModel);
		}


	}
}
