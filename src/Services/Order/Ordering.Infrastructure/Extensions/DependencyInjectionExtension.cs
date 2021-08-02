using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Repositories.Abstract;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Abstract;

namespace Ordering.Infrastructure.Extensions
{
	public static class DependencyInjectionExtension
	{
		private const string CONNECTION_STRING_NAME = "OrderConnection";
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddDbContext<OrderContext>(
			//	o => o.UseInMemoryDatabase(databaseName: "InMemoryDb"),
			//	ServiceLifetime.Singleton,
			//	ServiceLifetime.Singleton);

			services.AddDbContext<OrderContext>(
				o => o.UseSqlServer(configuration.GetConnectionString(CONNECTION_STRING_NAME),
					b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)), ServiceLifetime.Singleton);

			services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			services.AddTransient<IOrderRepository, OrderRepository>();

			return services;
		}
	}
}