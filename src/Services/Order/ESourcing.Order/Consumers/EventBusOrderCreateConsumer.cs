using System;
using System.Text;
using System.Text.Json;
using AutoMapper;
using EventBusRabbitMq;
using EventBusRabbitMq.Core;
using EventBusRabbitMq.Events;
using MediatR;
using Ordering.Application.Commands.OrderCreate;
using RabbitMQ.Client.Events;

namespace ESourcing.Order.Consumers
{
	public class EventBusOrderCreateConsumer
	{
		private readonly IRabbitMqPersistentConnection _persistentConnection;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public EventBusOrderCreateConsumer(IRabbitMqPersistentConnection persistentConnection, IMediator mediator, IMapper mapper)
		{
			_persistentConnection = persistentConnection;
			_mediator = mediator;
			_mapper = mapper;
		}

		public void Consume()
		{
			if (!_persistentConnection.IsConnected)
			{
				_persistentConnection.TryConnect();
			}

			var channel = _persistentConnection.CreateModel();
			channel.QueueDeclare(queue: EventBusConstants.ORDER_CREATE_QUEUE, false, false, false, null);

			var consumer = new EventingBasicConsumer(channel);

			consumer.Received += ReceivedEvent;
			channel.BasicConsume(queue: EventBusConstants.ORDER_CREATE_QUEUE, autoAck: true, consumer: consumer, consumerTag: "", noLocal: false,
				exclusive: false, arguments: null);
		}

		private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
		{
			var message = Encoding.UTF8.GetString(e.Body.Span);
			var @event = JsonSerializer.Deserialize<OrderCreateEvent>(message);

			if (e.RoutingKey != EventBusConstants.ORDER_CREATE_QUEUE) return;

			var command = _mapper.Map<OrderCreateCommand>(@event);
			command.CreatedAt = DateTime.Now;
			command.TotalPrice = @event.Price * @event.Quantity;
			command.UnitPrice = @event.Price;

			var result = await _mediator.Send(command);
		}

		public void Disconnect()
		{
			_persistentConnection.Dispose();
		}
	}
}
