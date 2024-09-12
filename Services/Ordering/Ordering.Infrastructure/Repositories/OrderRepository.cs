using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
      
        public OrderRepository(OrderContext orderContext) : base(orderContext) { }
       
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
           return await _orderContext.Orders.Where(d=>d.UserName == userName).ToListAsync();
        }
    }
}
