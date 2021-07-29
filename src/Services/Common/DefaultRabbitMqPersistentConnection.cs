using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMq
{
	public class DefaultRabbitMqPersistentConnection : IRabbitMqPersistentConnection
	{
		#region Variables

		private readonly IConnectionFactory _connectionFactory;
		private readonly int _retryCount;
		private readonly ILogger<DefaultRabbitMqPersistentConnection> _logger;
		private IConnection _connection;
		private bool _disposed;

		#endregion

		#region Properties

		public bool IsConnected => _connection is { IsOpen: true } && !_disposed;

		#endregion

		#region Constructor

		public DefaultRabbitMqPersistentConnection(IConnectionFactory connectionFactory,
			int retryCount,
			ILogger<DefaultRabbitMqPersistentConnection> logger)
		{
			_connectionFactory = connectionFactory;
			_retryCount = retryCount;
			_logger = logger;
		}

		#endregion

		#region Interface Implementations

		public bool TryConnect()
		{
			_logger.LogInformation("RabbitMQ Client is trying to connect");

			var policy = Policy.Handle<SocketException>()
				.Or<BrokerUnreachableException>()
				.WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
				{
					_logger.LogWarning(ex, "RabbitMQ Client cloud not connect after {TimeOut}s {{ExceptionMessage}}", $"{time.TotalSeconds:n1}", ex.Message);
				});

			policy.Execute(() =>
			{
				_connection = _connectionFactory.CreateConnection();
			});

			if (IsConnected)
			{
				_connection.ConnectionShutdown += OnConnectionShutDown;
				_connection.CallbackException += OnCallbackException;
				_connection.ConnectionBlocked += OnConnectionBlocked;

				_logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events",
					_connectionFactory.VirtualHost);

				return true;
			}

			_logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
			return false;
		}
		public IModel CreateModel()
		{
			if (!IsConnected)
			{
				throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
			}

			return _connection.CreateModel();
		}
		public void Dispose()
		{
			if (_disposed) return;
			_disposed = true;

			try
			{
				_connection.Dispose();
			}
			catch (IOException e)
			{
				_logger.LogCritical(e.ToString());
			}
		}

		#endregion

		#region RabbitMQ Events

		private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
		{
			if (_disposed) return;
			_logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

			TryConnect();
		}
		private void OnConnectionShutDown(object sender, ShutdownEventArgs e)
		{
			if (_disposed) return;
			_logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

			TryConnect();
		}
		private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
		{
			if (_disposed) return;
			_logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

			TryConnect();
		}

		#endregion
	}
}
