using ESourcing.Core.Entities;
using ESourcing.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESourcing.Infrastructure.Extensions
{
	public static class DependencyInjectionExtension
	{
		private const string CONNECTION_STRING_NAME = "WebAppConnection";
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<WebAppContext>(
				o => o.UseSqlServer(configuration.GetConnectionString(CONNECTION_STRING_NAME),
					b => b.MigrationsAssembly(typeof(WebAppContext).Assembly.FullName)), ServiceLifetime.Singleton);
			services.AddIdentity<AppUser, IdentityRole>(op =>
			{
				op.Password.RequiredLength = 4;
				op.Password.RequireNonAlphanumeric = false;
				op.Password.RequireLowercase = false;
				op.Password.RequireUppercase = false;
				op.Password.RequireDigit = false;
			}).AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();
			return services;
		}
	}
}
