using BlazorEcommerce.Data;
using BlazorEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ClearCartAsync(string? userId)
        {
            var cartItems = await _db.ShoppingCarts
                .Where(u => u.UserId == userId)
                .ToListAsync();
            if (cartItems.Count > 0)
            {
                _db.ShoppingCarts.RemoveRange(cartItems);
                return await SaveAsync();
            }
            return false;
        }
        
        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? userId)
        {
            return await _db.ShoppingCarts
                .Include(u => u.Product)
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }
        
        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateBy)
        {
            var cartFromDb = await _db.ShoppingCarts
                .FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);
            if (cartFromDb == null && updateBy > 0)
            {
                //create new cart item
                var newCartItem = new ShoppingCart()
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = updateBy
                };
                await _db.ShoppingCarts.AddAsync(newCartItem);
                return await SaveAsync();
            }
            else if (cartFromDb != null)
            {
                //update existing cart item
                cartFromDb.Quantity += updateBy;
                if (cartFromDb.Quantity <= 0)
                {
                    _db.ShoppingCarts.Remove(cartFromDb);
                }
                return await SaveAsync();
            }
            return false;
        }
        private async Task<bool> SaveAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> TruncateIfEmptyAsync()
        {
            var any = await _db.ShoppingCarts.AnyAsync();
            if (!any)
            {
                await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [ShoppingCarts]");
                return true;
            }
            return false;
        }

        public async Task<int> GetCartCountAsync(string? userId)
        {
            return await _db.ShoppingCarts
                .Where(u => u.UserId == userId)
                .SumAsync(u => u.Quantity);
        }
    }
}
