using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Domain.Entities;

namespace Ordering.Domain.Repositories.Abstract
{
	public interface IOrderRepository:IRepository<Order>
	{
		Task<IEnumerable<Order>> GetOrdersBySellerUserName(string userName);
	}
}