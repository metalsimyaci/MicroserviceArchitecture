using ESourcing.Sourcing.Data;
using ESourcing.Sourcing.Data.Interface;
using ESourcing.Sourcing.Repositories;
using ESourcing.Sourcing.Repositories.Interfaces;
using ESourcing.Sourcing.Settings;
using EventBusRabbitMq;
using EventBusRabbitMq.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace ESourcing.Sourcing
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
		    #region Configuration Dependencies

		    services.Configure<SourcingDatabaseSettings>(Configuration.GetSection(nameof(SourcingDatabaseSettings)));
		    services.AddSingleton<ISourcingDatabaseSettings>(sp => sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);

		    #endregion

		    #region Project Dependencies

		    services.AddTransient<ISourcingContext, SourcingContext>();
		    services.AddTransient<IAuctionRepository, AuctionRepository>();
		    services.AddTransient<IBidRepository, BidRepository>();
		    services.AddAutoMapper(typeof(Startup));

		    #endregion

		    #region Swagger Dependencies

		    services.AddControllers();
		    services.AddSwaggerGen(s =>
		    {
			    s.SwaggerDoc("v1", new OpenApiInfo {Title = "ESourcing.Sourcing", Version = "v1"});
		    });

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

				if(!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
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
			services.AddSingleton<EventBusRabbitMqProducer>();

			#endregion
	    }

	    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "Sourcing API V1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
