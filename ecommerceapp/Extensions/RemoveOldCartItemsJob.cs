using ecommerceapp.Data;
using Quartz;

namespace ecommerceapp.Extensions
{
    public class RemoveOldCartItemsJob : IJob
    {
        private readonly ApplicationDbContext _context;

        public RemoveOldCartItemsJob(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var olderThan = DateTime.UtcNow.AddMinutes(-1);
            var cartItems = _context.shoppingCarts
                .Where(c => c.Timestamp < olderThan)
                .ToList();
            _context.shoppingCarts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
