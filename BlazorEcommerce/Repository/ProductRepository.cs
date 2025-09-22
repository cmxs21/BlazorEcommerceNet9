using BlazorEcommerce.Data;
using BlazorEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> CreateAsync(Product obj)
        {
            await _db.Products.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (product == null)
            {
                return false;
            }
            _db.Products.Remove(product);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<Product?> GetAsync(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product obj)
        {
            var product = await _db.Products.FirstOrDefaultAsync(c => c.Id == obj.Id);
            if (product == null)
            {
                return obj;
            }
            product.Name = obj.Name;
            product.Description = obj.Description;
            product.Price = obj.Price;
            product.CategoryId = obj.CategoryId;
            product.ImageUrl = obj.ImageUrl;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
