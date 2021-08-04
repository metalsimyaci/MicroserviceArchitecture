using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESourcing.Infrastructure.Extensions;
using ESourcing.UI.Clients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ESourcing.UI
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddInfrastructure(Configuration);
			services.AddMvc();
			services.AddControllersWithViews();

			services.AddHttpClient();
			services.AddHttpClient<ProductClient>();
			services.AddHttpClient<AuctionClient>();
			services.AddHttpClient<BidClient>();

			//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			//	.AddCookie(op =>
			//{
			//	op.Cookie.Name = "DestCookie";
			//	op.LoginPath = "Home/Login";
			//	op.LogoutPath = "Home/Logout";
			//	op.ExpireTimeSpan = TimeSpan.FromDays(3);
			//	op.SlidingExpiration = false;
			//});
			services.ConfigureApplicationCookie(op =>
			{
				op.LoginPath = $"/Home/Login";
				op.LogoutPath = $"/Home/Logout";
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}