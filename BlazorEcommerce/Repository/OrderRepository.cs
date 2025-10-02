using BlazorEcommerce.Data;
using BlazorEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<OrderHeader> CreateAsync(OrderHeader orderHeader)
        {
            orderHeader.OrderDate = DateTime.Now;
            await _db.OrderHeaders.AddAsync(orderHeader);
            await _db.SaveChangesAsync();
            return orderHeader;
        }
        public async Task<IEnumerable<OrderHeader>> GetAllAsync(string? userId = null)
        {
            if (userId == null)
            {
                return await _db.OrderHeaders.ToListAsync();
            }
            return await _db.OrderHeaders.Where(oh => oh.UserId == userId).ToListAsync();
        }
        public async Task<OrderHeader> GetAsync(int id)
        {
            return await _db.OrderHeaders
                .Include(oh => oh.OrderDetails)
                .ThenInclude(op => op.Product)
                .FirstAsync(oh => oh.Id == id);
        }
        public async Task<OrderHeader> UpdateStatusAsync(int orderId, string status)
        {
            var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(oh => oh.Id == orderId);
            if (orderHeader != null)
            {
                orderHeader.OrderStatus = status;
                await _db.SaveChangesAsync();
            }
            return orderHeader;
        }
    }
}
