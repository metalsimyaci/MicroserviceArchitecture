using System;
using RabbitMQ.Client;

namespace EventBusRabbitMq
{
	public interface IRabbitMqPersistentConnection:IDisposable
	{
		public bool IsConnected { get; }
		public bool TryConnect();
		IModel CreateModel();
	}
}