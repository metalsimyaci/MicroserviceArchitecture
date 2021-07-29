using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using EventBusRabbitMq.Events.Abstracts;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMq.Producer
{
	public class EventBusRabbitMqProducer
	{
		private readonly IRabbitMqPersistentConnection _persistentConnection;
		private readonly ILogger<EventBusRabbitMqProducer> _logger;
		private readonly int _retryCount;

		public EventBusRabbitMqProducer(IRabbitMqPersistentConnection persistentConnection, ILogger<EventBusRabbitMqProducer> logger, int retryCount=3)
		{
			_persistentConnection = persistentConnection;
			_logger = logger;
			_retryCount = retryCount;
		}

		public void Publish(string queueName, EventBase @event)
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}

			var policy = Policy.Handle<BrokerUnreachableException>()
				.Or<SocketException>()
				.WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
				{
					_logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s {{ExceptionMessage}}", @event.RequestId,
						$"{time.TotalSeconds}", ex.Message);
				});

			using var channel = _persistentConnection.CreateModel();
			channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
			var message = JsonSerializer.Serialize(@event);
			var body = Encoding.UTF8.GetBytes(message);

			policy.Execute(() =>
			{
				var properties = channel.CreateBasicProperties();
				properties.Persistent = true;
				properties.DeliveryMode = 2;

				channel.ConfirmSelect();
				channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
				channel.WaitForConfirmsOrDie();

				channel.BasicAcks += (sender, eventArgs) =>
				{
					Console.WriteLine("Sent RabbitMQ");
				};
			});
		}
	}
}
