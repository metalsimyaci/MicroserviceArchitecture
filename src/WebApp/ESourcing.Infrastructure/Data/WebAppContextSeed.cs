using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESourcing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESourcing.Infrastructure.Data
{
	public class WebAppContextSeed
	{
		private const int MAX_RETRY_COUNT = 3;
		public static async Task SeedAsync(WebAppContext context, ILoggerFactory loggerFactory, int retry=0)
		{
			var retryAvailability = retry;
			try
			{
				await context.Database.MigrateAsync();
				if (!context.AppUsers.Any())
				{
					context.AppUsers.AddRange(GetPreConfiguredUsers());
					await context.SaveChangesAsync();
				}
			}
			catch (Exception e)
			{
				if (retryAvailability <= MAX_RETRY_COUNT)
				{
					retryAvailability++;
					var log = loggerFactory.CreateLogger<WebAppContextSeed>();
					log.LogError(e.Message);
					Thread.Sleep(2000);
					await SeedAsync(context, loggerFactory, retryAvailability);
				}
			}
		}

		private static IEnumerable<AppUser> GetPreConfiguredUsers()
		{
			return new List<AppUser>
			{
				new AppUser
				{
					FirstName ="User1",
					LastName = "User LastName1",
					IsSeller = true,
					IsBuyer = false
				}
			};
		}
	}
}
