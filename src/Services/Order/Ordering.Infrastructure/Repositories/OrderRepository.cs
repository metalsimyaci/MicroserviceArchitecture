using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories.Abstract;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories.Abstract;

namespace Ordering.Infrastructure.Repositories
{
	public class OrderRepository:Repository<Order>,IOrderRepository
	{
		public OrderRepository(OrderContext dbContext) : base(dbContext)
		{
		}

		public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName)
		{
			return await _dbContext.Orders.Where(x => x.SellerUserName == userName).ToListAsync();
		}
	}
}