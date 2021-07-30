using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Data
{
	public static class OrderContextSeed
	{
		public static async Task SeedAsync(OrderContext orderContext)
		{
			if (!orderContext.Orders.Any())
			{
				orderContext.Orders.AddRange(GetPreConfiguredOrders());
				await orderContext.SaveChangesAsync();
			}
		}
		private static IEnumerable<Order> GetPreConfiguredOrders()
		{
			return new List<Order>
			{
				new Order
				{
					AuctionId = Guid.NewGuid().ToString(),
					ProductionId = Guid.NewGuid().ToString(),
					SellerUserName = "test@test.com",
					UnitPrice = 10,
					TotalPrice = 1000,
					CreatedAt = DateTime.UtcNow
				},
				new Order
				{
					AuctionId = Guid.NewGuid().ToString(),
					ProductionId = Guid.NewGuid().ToString(),
					SellerUserName = "test1@test.com",
					UnitPrice = 10,
					TotalPrice = 1000,
					CreatedAt = DateTime.UtcNow
				},
				new Order
				{
					AuctionId = Guid.NewGuid().ToString(),
					ProductionId = Guid.NewGuid().ToString(),
					SellerUserName = "test2@test.com",
					UnitPrice = 10,
					TotalPrice = 1000,
					CreatedAt = DateTime.UtcNow
				},
			};
		}
	}
}