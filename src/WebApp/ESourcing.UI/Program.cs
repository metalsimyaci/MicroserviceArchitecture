using System;
using ESourcing.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace ESourcing.UI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();
			CreateAndSeedDatabase(host);
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

		private static void CreateAndSeedDatabase(IHost host)
		{
			using var scope = host.Services.CreateScope();

			var serviceProvider = scope.ServiceProvider;
			var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
			
			try
			{
				var context = serviceProvider.GetRequiredService<WebAppContext>();
				WebAppContextSeed.SeedAsync(context, loggerFactory).Wait();
			}
			catch (Exception e)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(e,"An error occurred seeding in DB");
			}
		}
	}
}