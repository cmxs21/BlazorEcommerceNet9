using BlazorEcommerce.Data;

namespace BlazorEcommerce.Repository.IRepository
{
    public interface IProductRepository
    {
        public Task<Product> CreateAsync(Product obj);
        public Task<Product> UpdateAsync(Product obj);
        public Task<Product?> GetAsync(int id);
        public Task<IEnumerable<Product>> GetAllAsync();
        public Task<bool> DeleteAsync(int id);
    }
}
