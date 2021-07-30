using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Data
{
	public class OrderContext:DbContext
	{
		public DbSet<Order> Orders { get; set; }

		public OrderContext(DbContextOptions<OrderContext> options):base(options)
		{
			
		}
	}
}