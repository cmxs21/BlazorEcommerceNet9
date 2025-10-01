using BlazorEcommerce.Data;

namespace BlazorEcommerce.Repository.IRepository
{
    public interface IShoppingCartRepository
    {
        public Task<bool> UpdateCartAsync(string userId, int product, int updateBy); //updateBy can be positive or negative to increment or decrement
        public Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId);
        public Task<bool> ClearCartAsync(string? userId);
        public Task<bool> TruncateIfEmptyAsync();
    }
}
