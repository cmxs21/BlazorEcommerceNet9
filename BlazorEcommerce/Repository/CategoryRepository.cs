using BlazorEcommerce.Data;
using BlazorEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> CreateAsync(Category obj)
        {
            await _db.Categories.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return false;
            }
            _db.Categories.Remove(category);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<Category?> GetAsync(int id)
        {
            return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category obj)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == obj.Id);
            if (category == null)
            {
                return obj;
            }
            category.Name = obj.Name;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}
