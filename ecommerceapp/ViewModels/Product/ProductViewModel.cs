using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;

namespace ecommerceapp.ViewModels.Product
{
    public class ProductViewModel
    {
        private readonly ApplicationDbContext _db;

        public ProductViewModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }


        public List<ecommerceapp.Models.Product> GetProducts()
        {
            var products = from p in _db.product
                           join c in _db.category on p.cId equals c.CategoryId into proGroup
                           from d in proGroup.DefaultIfEmpty()
                           select new ecommerceapp.Models.Product
                           {
                               productId = p.productId,
                               productName = p.productName,
                               productDesc = p.productDesc,
                               productPrice = p.productPrice,
                               prodimg = p.prodimg,
                               category = d.CategoryName
                           };
            return products.ToList();
        }



        public ecommerceapp.Models.Product GetProductById(int id)
        {
            return _db.product.FirstOrDefault(p => p.productId == id);
        }


        public List<ecommerceapp.Models.Product> GetAllProducts()
        {
            return _db.product.ToList();
        }
        public List<Categories> GetAllCategories()
        {
            return _db.category.ToList();
        }


        public void RemoveProduct(int productId)
        {
            var product = _db.product.FirstOrDefault(c => c.productId == productId);
            if (product != null)
            {
                _db.product.Remove(product);
                _db.SaveChanges();
            }
        }


        public bool IsImageExist(IFormFile productimg)
        {
            return _db.product.Where(x => x.prodimg == productimg.FileName).Any();
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task AddProductAsync(ecommerceapp.Models.Product obj)
        {
            _db.product.Add(obj);
            await _db.SaveChangesAsync();
        }

        //public async Task UpdateProductAsync(ecommerceapp.Models.Product obj)
        //{

        //    _db.product.Update(obj);
        //    await _db.SaveChangesAsync();
        //}
        public async Task<List<Categories>> GetAllCategoriesAsync()
        {
            return await _db.category.ToListAsync();
        }

        public async Task UpdateProductAsync(ecommerceapp.Models.Product obj, List<Categories> categories)
        {
            obj.categories = categories;
            _db.product.Update(obj);
            await _db.SaveChangesAsync();
        }



        


    }

}
