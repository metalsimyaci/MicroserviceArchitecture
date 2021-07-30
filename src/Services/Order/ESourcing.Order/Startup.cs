using ESourcing.Order.Consumers;
using ESourcing.Order.Extensions;
using EventBusRabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;
using RabbitMQ.Client;

namespace ESourcing.Order
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
	        services.AddApplication();
            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            #region Swagger Dependencies

            services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo() {Title = "ESourcing.Order", Version = "v1"}));

			#endregion

			#region EventBus

			services.AddSingleton<IRabbitMqPersistentConnection>(s =>
			{
				var logger = s.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
				var factory = new ConnectionFactory()
				{
					HostName = Configuration["EventBus:HostName"]
				};

				if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
				{
					factory.UserName = Configuration["EventBus:UserName"];
				}

				if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
				{
					factory.Password = Configuration["EventBus:Password"];
				}


				var retryCount = 3;
				if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
				{
					retryCount = int.Parse(Configuration["EventBus:RetryCount"]);
				}

				return new DefaultRabbitMqPersistentConnection(factory, retryCount, logger);
			});
			services.AddSingleton<EventBusOrderCreateConsumer>();

			#endregion
        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1"));
            }

            app.UseEventBusListener();

			app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
