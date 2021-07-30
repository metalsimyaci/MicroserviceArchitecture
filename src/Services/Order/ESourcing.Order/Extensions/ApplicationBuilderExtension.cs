using ESourcing.Order.Consumers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ESourcing.Order.Extensions
{
	public static class ApplicationBuilderExtension
	{
		private static EventBusOrderCreateConsumer Listener { get; set; }

		public static IApplicationBuilder UseEventBusListener(this IApplicationBuilder app)
		{
			Listener = app.ApplicationServices.GetService<EventBusOrderCreateConsumer>();
			var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

			life.ApplicationStarted.Register(OnStarted);
			life.ApplicationStopping.Register(OnStopped);

			return app;
		}

		private static void OnStarted()
		{
			Listener.Consume();
		}

		private static void OnStopped()
		{
			Listener.Disconnect();
		}
	}
}
