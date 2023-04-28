using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerceapp.ViewModels.View
{
    public class ViewViewModel
    {
        private readonly ApplicationDbContext _db;

        public ViewViewModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return _db.category.ToList();
        }
        public async Task<Categories> GetCategoryByIdAsync(int id)
        {
            return await _db.category.FindAsync(id);
        }

        public IEnumerable<ecommerceapp.Models.Product> GetProductsByCategoryId(int categoryId)
        {
            return _db.product.Where(p => p.cId == categoryId).ToList();
        }

        public IEnumerable<ShoppingCart> GetCartItemsByUserId(string userId)
        {
            return _db.shoppingCarts.Include(c => c.Product).Where(c => c.RegisterUserId == userId).ToList();
        }
        //public IEnumerable<ShopCartVM> GetCartVMItemsByUserId(string userId)
        //{
        //    return _db.shopCartVMs.Include(c => c.Product).ToList();
        //}

		public IEnumerable<ShoppingCart> GetCartVMItemsByUserId()
		{
			return _db.shoppingCarts.Include(c => c.Product).ToList();
		}

		public ecommerceapp.Models.Product GetProductById(int id)
        {
            return _db.product.FirstOrDefault(p => p.productId == id);
        }
        //-------------------------------------------
        public ShopCartVM GetItemId(int id)
        {
            return _db.shopCartVMs.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddRangeToShopCartVMsAsync(IEnumerable<ShopCartVM> shopCartVMs)
        {
            _db.shopCartVMs.AddRange(shopCartVMs);
            await _db.SaveChangesAsync();
        }
        public async Task AddShoppingCartAsync(ShoppingCart cartObj)
        {
            _db.shoppingCarts.Add(cartObj);
            await _db.SaveChangesAsync();
        }
        
        public async Task IncrementCartItemQuantityAsync(int cartId)
        {
            var cartItem = await _db.shoppingCarts.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                await _db.SaveChangesAsync();
            }
        }
        public void DecreaseCartItem(int cartId)
        {
            var cartItem = _db.shoppingCarts.FirstOrDefault(c => c.Id == cartId);
            if (cartItem != null)
            {
                cartItem.Quantity--;
                if (cartItem.Quantity == 0)
                {
                    _db.shoppingCarts.Remove(cartItem);
                }
                _db.SaveChanges();
            }
        }
        public async Task RemoveCartItemAsync(int cartId)
        {
            var itemToRemove = await _db.shoppingCarts.FindAsync(cartId);
            if (itemToRemove != null)
            {
                _db.shoppingCarts.Remove(itemToRemove);
                await _db.SaveChangesAsync();
            }
        }
        public async Task RemoveShopCartVM(int cartId)
        {
            var itemToRemove = _db.shopCartVMs.Find(cartId);
            if (itemToRemove != null)
            {
                _db.shopCartVMs.Remove(itemToRemove);
                await _db.SaveChangesAsync();
            }
        }


        public async Task AddShoppingCartToAsync(ShoppingCart shoppingCart)
        {
            var existingCartItem = _db.shoppingCarts.FirstOrDefault(c => c.RegisterUserId == shoppingCart.RegisterUserId && c.productId == shoppingCart.productId);

            if (existingCartItem != null)
            {
                // If the product already exists in the cart, increment the quantity
                existingCartItem.Quantity += shoppingCart.Quantity;
            }
            else
            {
                // If the product does not exist in the cart, add a new entry
                _db.shoppingCarts.Add(shoppingCart);
            }

            await _db.SaveChangesAsync();
        }

        public List<ShopCartVM> GetShopCartVMById(int id)
        {
            return _db.shopCartVMs.Where(x => x.Id == id).ToList();
        }
        public List<ShopCartVM> GetShopCartItems()
        {
            var items = _db.shopCartVMs.ToList();
            return items;
        }


    }
}
