using BlazorEcommerce.Data;

namespace BlazorEcommerce.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<Category> CreateAsync(Category obj);
        public Task<Category> UpdateAsync(Category obj);
        public Task<Category?> GetAsync(int id);
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<bool> DeleteAsync(int id);
    }
}
