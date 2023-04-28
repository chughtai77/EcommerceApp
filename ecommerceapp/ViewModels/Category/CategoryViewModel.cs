using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerceapp.ViewModels.Category
{
    public class CategoryViewModel
    {
        private readonly ApplicationDbContext _db;

        public CategoryViewModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public IEnumerable<Categories> GetAllCategories()
        {
            return _db.category.ToList();
        }
        public Categories GetCategoryById(int id)
        {
            return _db.category.FirstOrDefault(p => p.CategoryId == id);
        }

        public void RemoveProduct(int categoryId)
        {
            var category = _db.category.FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                _db.category.Remove(category);
                _db.SaveChanges();
            }
        }

        public bool IsImageExist(IFormFile categoryimg)
        {
            return _db.category.Where(x => x.img == categoryimg.FileName).Any();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(Categories obj)
        {
            _db.category.Add(obj);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Categories obj)
        {
            _db.category.Update(obj);
            await _db.SaveChangesAsync();
        }
    }
}