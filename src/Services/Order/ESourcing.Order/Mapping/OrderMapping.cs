using AutoMapper;
using EventBusRabbitMq.Events;
using Ordering.Application.Commands.OrderCreate;

namespace ESourcing.Order.Mapping
{
	public class OrderMapping:Profile
	{
		public OrderMapping()
		{
			CreateMap<OrderCreateEvent, OrderCreateCommand>().ReverseMap();
		}
	}
}
